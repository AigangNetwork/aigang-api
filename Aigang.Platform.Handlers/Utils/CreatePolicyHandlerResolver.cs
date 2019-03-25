using System;
using System.Threading.Tasks;
using Aigang.Platform.Contracts.Insurance.Requests;
using Aigang.Platform.Contracts.Insurance.Responses;
using Aigang.Platform.Domain.Insurance;
using Aigang.Platform.Handlers.Base;
using log4net;

namespace Aigang.Platform.Handlers.Utils
{
    public class CreatePolicyHandlerResolver
    {
        private CreateAndroidPolicyHandler _createAndroidPolicyHandler;
        
        public CreatePolicyHandlerResolver(ILog logger)
        {
            _createAndroidPolicyHandler = new CreateAndroidPolicyHandler(logger);
        }
        
        public HandlerBase<CreatePolicyRequest, CreatePolicyResponse> GetHandler(ProductType productTypeId)
        {
            switch (productTypeId)
            {
                case ProductType.AndroidDeviceInsurance :
                    return _createAndroidPolicyHandler;
                default: 
                    return null;
            }
        }
    }
}