using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Aigang.AndroidDataCollector.Client;
using Aigang.AndroidDataCollector.Client.Responses;
using Aigang.Contracts.Executor.Api.Client;
using Aigang.Platform.Contracts.Errors;
using Aigang.Platform.Contracts.Insurance.Requests;
using Aigang.Platform.Contracts.Insurance.Responses;
using Aigang.Platform.Domain.Base;
using Aigang.Platform.Domain.DeviceData;
using Aigang.Platform.Domain.Insurance;
using Aigang.Platform.Handlers.Base;
using Aigang.Platform.Repository.InsuranceRepository;
using AutoMapper;
using log4net;
using Newtonsoft.Json;

namespace Aigang.Platform.Handlers
{
    public class VerifyAndroidPolicyHandler : HandlerBase<VerifyPolicyClaimRequest, VerifyPolicyClaimResponse>
    {
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IAndroidDataCollectorClient _androidDataCollectorClient;
        private Policy _policy;
        
        public VerifyAndroidPolicyHandler(ILog logger) : base(logger)
        {
            _androidDataCollectorClient = new AndroidDataCollectorClient();
            _insuranceRepository = new InsuranceRepository();
        }

        protected override async Task<List<ValidationFailure>> ValidateRequestAsync(VerifyPolicyClaimRequest request)
        {
            var errors = new List<ValidationFailure>();
            
            if (string.IsNullOrWhiteSpace(request.TaskId))
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Task id is not set");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(request.PolicyId))
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Policy id is not set");
                return errors;
            }
            
            if (string.IsNullOrWhiteSpace(request.ProductAddress))
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Product address is not set");
                return errors;
            }
            
            if (request.ProductTypeId != (int)ProductType.AndroidDeviceInsurance)
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Product Type id is not valid");
                return errors;
            }
            
            _policy = await ContractsExecutorClient.GetPolicy(request.ProductAddress, request.ProductTypeId, request.PolicyId);
           
            
            if (_policy == null)
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Policy Id is not valid");
                return errors;
            }
            if (_policy.Premium == 0)
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Policy Premium is not valid");
                return errors;
            }
            
            DateTime now = new DateTime();

            if (_policy.StartUtc < now && _policy.EndUtc >= now)
            {
                errors.AddError(ValidationErrorReasons.ValidationFailed, "Policy is ended");
                return errors;
            }
            
            if (_policy.IsCanceled)
            {
                errors.AddError(ValidationErrorReasons.ValidationFailed, "Policy is canceled");
                return errors;
            }
            
            if (_policy.Payout != 0)
            {
                errors.AddError(ValidationErrorReasons.ValidationFailed, "Policy already paid out");
                return errors;
            }
            
            ProductStats stats = await ContractsExecutorClient.GetProductStats(request.ProductAddress, request.ProductTypeId);
            
            if (stats.Paused)
            {
                errors.AddError(ValidationErrorReasons.ValidationFailed, "Product is paused");
                return errors;
            }
            
            
            // TODO: validate wallet address? that someone do not fake ?
            
            return errors;
        }
    
        protected override async Task<VerifyPolicyClaimResponse> HandleCoreAsync(VerifyPolicyClaimRequest request)
        {
            var response = new VerifyPolicyClaimResponse();

            MobileDataResponse dataCollectorResponse = await _androidDataCollectorClient.GetDeviceDataAsync(request.TaskId);
            
            if (dataCollectorResponse.IsResponseAccepted)
            {
                response.SuccessStatusCode = HttpStatusCode.Accepted;
                return response;
            }
            
            var mobileData = Mapper.Map<MobileDataResponse, MobileData>(dataCollectorResponse);
            _policy.DeviceId = await _insuranceRepository.GetPolicyDeviceId(request.PolicyId);

            if (mobileData.DeviceId != _policy.DeviceId)
            {
                throw new ValidationFailedException(new ErrorResponse{
                    Reason = ValidationErrorReasons.BadRequest,
                    Message = "Mobile device id does not match policy device id."
                });
            }
            
            response.IsTaskFinished = true;

            
            response.IsClaimable = await ContractsExecutorClient.IsClaimableAsync(request.ProductTypeId, request.ProductAddress, mobileData);
            
            if (response.IsClaimable)
            {    
                var claimProperties = new MobileDataProperties
                {
                    WL = mobileData.WearLevel.ToString("D"),
                    DB = mobileData.Brand,
                    R = mobileData.Region,
                    DA = mobileData.AgeInMonths,
                    CL = mobileData.ChargeLevel,
                    DC = mobileData.BatteryDesignCapacity
                };
              
                _policy.ClaimProperties = JsonConvert.SerializeObject(claimProperties);
                _policy.Status = PolicyStatus.Claimable;  
                
                await _insuranceRepository.UpdatePolicyAsync(_policy);
            }            

            return response;
        }
    }
}