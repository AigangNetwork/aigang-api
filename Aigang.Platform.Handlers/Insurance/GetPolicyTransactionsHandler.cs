using System.Collections.Generic;
using System.Threading.Tasks;
using Aigang.Platform.Contracts.Errors;
using Aigang.Platform.Contracts.Insurance;
using Aigang.Platform.Contracts.Insurance.Requests;
using Aigang.Platform.Contracts.Insurance.Responses;
using Aigang.Platform.Domain.Base;
using Aigang.Platform.Domain.Insurance;
using Aigang.Platform.Handlers.Base;
using Aigang.Platform.Repository.InsuranceRepository;
using AutoMapper;
using log4net;


namespace Aigang.Platform.Handlers

{
    public class GetPolicyTransactionsHandler : HandlerBase<BasePolicyRequest, GetPolicyTransactionsResponse>
    {
        private readonly IInsuranceRepository _insuranceRepository;
        
        public GetPolicyTransactionsHandler(ILog logger) : base(logger)
        {
            _insuranceRepository = new InsuranceRepository();
        }

        protected override async Task<List<ValidationFailure>> ValidateRequestAsync(BasePolicyRequest request)
        {
            var errors = new List<ValidationFailure>();

            if (string.IsNullOrWhiteSpace(request.PolicyId))
            {
                errors.AddError(ValidationErrorReasons.InvalidQuery, "Policy id is not set");
                return errors;
            }
            
            return errors;
        }
    
        protected override async Task<GetPolicyTransactionsResponse> HandleCoreAsync(BasePolicyRequest request)
        {
            var response = new GetPolicyTransactionsResponse();

            var policyTransactions = await _insuranceRepository.GetPolicyTransactions(request.PolicyId);

            response.PolicyTransactions = Mapper.Map<PolicyTransactions, PolicyTransactionsDto>(policyTransactions);
            
            return response;
        }
    }
}