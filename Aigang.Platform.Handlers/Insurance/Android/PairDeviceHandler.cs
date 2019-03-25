using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Aigang.AndroidDataCollector.Client;
using Aigang.Contracts.Executor.Api.Client;
using Aigang.Platform.Contracts.Errors;
using Aigang.Platform.Contracts.Insurance.Android;
using Aigang.Platform.Domain.Base;
using Aigang.Platform.Domain.Insurance;
using Aigang.Platform.Handlers.Base;
using Aigang.Platform.Handlers.Utils;
using Aigang.Platform.Repository.InsuranceRepository;
using log4net;

namespace Aigang.Platform.Handlers.Insurance.Android
{
    public class PairDeviceHandler : HandlerBase<PairDeviceRequest, PairDeviceResponse>
    {
        private readonly IAndroidDataCollectorClient _androidDataCollectorClient;
     
        
        public PairDeviceHandler(ILog logger) : base(logger)
        {
            _androidDataCollectorClient = new AndroidDataCollectorClient();
        }
        
        protected override async Task<List<ValidationFailure>> ValidateRequestAsync(PairDeviceRequest request)
        {
            var errors = new List<ValidationFailure>();
            
            if (string.IsNullOrWhiteSpace(request.ProductAddress))
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "ProductAddress id is not set");
                return errors;
            }
            
            if (request.ProductTypeId == 0)
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "ProductTypeId id is not set");
                return errors;
            }
            
            if (string.IsNullOrWhiteSpace(request.DeviceId))
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Device id is not set");
                return errors;
            }
            
            if (request.DeviceId.Length != 8)
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Device id is not 8 characters long");
                return errors;
            }
            
            if (request.ProductTypeId != (int)ProductType.AndroidDeviceInsurance)
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Product Type id is not valid");
                return errors;
            }

            Product product = await CachedProducts.GetProductAsync(request.ProductTypeId, request.ProductAddress);

            if (!ProductValidator.IsProductActive(product))
            {
                errors.AddError(ValidationErrorReasons.ValidationFailed, "Product is not active");
                return errors;
            }
            
            return errors;
        }

        protected override async Task<PairDeviceResponse> HandleCoreAsync(PairDeviceRequest request)
        {
            var response = new PairDeviceResponse();
            
            var requestDataResponse = await _androidDataCollectorClient.RequestForDeviceData(request.DeviceId);
            
            if (requestDataResponse.IsResponseAccepted)
            {
                response.SuccessStatusCode = HttpStatusCode.Accepted;
                response.TaskId = requestDataResponse.TaskId;
            }
            
            return response;
        }
    }
}