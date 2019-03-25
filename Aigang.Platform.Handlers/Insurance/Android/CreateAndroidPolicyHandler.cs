using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Aigang.AndroidDataCollector.Client;
using Aigang.AndroidDataCollector.Client.Responses;
using Aigang.Contracts.Executor.Api.Client;
using Aigang.Platform.Contracts.Errors;
using Aigang.Platform.Contracts.Insurance;
using Aigang.Platform.Contracts.Insurance.Requests;
using Aigang.Platform.Contracts.Insurance.Responses;
using Aigang.Platform.Domain.Base;
using Aigang.Platform.Domain.DeviceData;
using Aigang.Platform.Domain.Insurance;
using Aigang.Platform.Handlers.Base;
using Aigang.Platform.Handlers.Utils;
using Aigang.Platform.Repository.InsuranceRepository;
using AutoMapper;
using log4net;
using Newtonsoft.Json;

namespace Aigang.Platform.Handlers

{
    public class CreateAndroidPolicyHandler : HandlerBase<CreatePolicyRequest, CreatePolicyResponse>
    {
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IAndroidDataCollectorClient _androidDataCollectorClient;
        private Product _product;
        
        public CreateAndroidPolicyHandler(ILog logger) : base(logger)
        {
            _androidDataCollectorClient = new AndroidDataCollectorClient();
            _insuranceRepository = new InsuranceRepository();
        }

        protected override async Task<List<ValidationFailure>> ValidateRequestAsync(CreatePolicyRequest request)
        {
            var errors = new List<ValidationFailure>();

            if (string.IsNullOrWhiteSpace(request.TaskId))
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Task id is not set");
                return errors;
            }
            
            if (string.IsNullOrWhiteSpace(request.DeviceId))
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Device Id is not set");
                return errors;
            }
            
            if (string.IsNullOrWhiteSpace(request.ProductAddress))
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "ProductAddress is not set");
                return errors;
            }
            
            if (request.ProductTypeId != (int)ProductType.AndroidDeviceInsurance)
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Product Type id is not valid");
                return errors;
            }
            
            _product = await CachedProducts.GetProductAsync(request.ProductTypeId, request.ProductAddress);
            
            if (!ProductValidator.IsProductActive(_product))
            {
                errors.AddError(ValidationErrorReasons.ValidationFailed, "Product is not active");
                return errors;
            }
            
            ProductStats stats = await ContractsExecutorClient.GetProductStats(request.ProductAddress, request.ProductTypeId);
            
            if (stats.Paused)
            {
                errors.AddError(ValidationErrorReasons.ValidationFailed, "Product is paused");
                return errors;
            }
            
            if (stats.PoliciesLength >= _product.PoliciesLimit)
            {
                errors.AddError(ValidationErrorReasons.ValidationFailed, "Product policies limit was reached");
                return errors;
            }
            
            if (stats.ProductTokensPool >= _product.ProductPoolLimit)
            {
                errors.AddError(ValidationErrorReasons.ValidationFailed, "Product pool limit was reached");
                return errors;
            }
            
            return errors;
        }
    
        protected override async Task<CreatePolicyResponse> HandleCoreAsync(CreatePolicyRequest request)
        {
            var response = new CreatePolicyResponse();

            var dataCollectorResponse = await _androidDataCollectorClient.GetDeviceDataAsync(request.TaskId);
            
            if (dataCollectorResponse.IsResponseAccepted)
            {
                response.SuccessStatusCode = HttpStatusCode.Accepted;
                return response;
            }
            
            var mobileData = Mapper.Map<MobileDataResponse, MobileData>(dataCollectorResponse);
            
            
            var validationResultCode = await ContractsExecutorClient.ValidateDataAsync(request.ProductTypeId, _product.Address, mobileData);
            
            if (!string.IsNullOrEmpty(validationResultCode))
            {
                response.ValidationResultCode = validationResultCode;
                return response;
            }
            
            var premium = await ContractsExecutorClient.CalculatePremiumAsync(request.ProductTypeId, _product.Address, mobileData);

            var product = await CachedProducts.GetProductAsync(request.ProductTypeId, _product.Address);
            
            var properties = new MobileDataProperties
            {
                WL = mobileData.WearLevel.ToString("D"),
                DB = mobileData.Brand,
                R = mobileData.Region,
                DA = mobileData.AgeInMonths,
                CL = mobileData.ChargeLevel,
                DC = mobileData.BatteryDesignCapacity
            };
            
            Policy policy = new Policy
            {
                DeviceId = mobileData.DeviceId,
                Status = PolicyStatus.Draft,
                Premium = premium,
                Payout = product.Payout,
                Fee = product.Loading,
                Properties = JsonConvert.SerializeObject(properties),
                ProductAddress = request.ProductAddress,
                ProductTypeId = (ProductType) request.ProductTypeId
            };

            policy = await _insuranceRepository.InsertPolicyAsync(policy);

            response.Policy = Mapper.Map<Policy, PolicyDto>(policy);
            
            return response;
        }
    }
}