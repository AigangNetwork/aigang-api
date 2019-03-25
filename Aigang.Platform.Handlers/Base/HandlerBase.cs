using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aigang.Platform.Contracts;
using Aigang.Platform.Contracts.Errors;
using Aigang.Platform.Domain.Base;
using log4net;
using Newtonsoft.Json;

namespace Aigang.Platform.Handlers.Base
{
    public abstract class HandlerBase<TRequest, TResponse> where TRequest : BaseRequest where TResponse : BaseResponse, new()
    {
        public readonly ILog Logger;

        protected HandlerBase(ILog logger)
        {
            Logger = logger;
        }

        public async Task<TResponse> HandleAsync(TRequest request)
        {         
            TResponse response;

            try
            {
                List<ValidationFailure> failures = await ValidateRequestWrapperAsync(request);

                if (failures.Any())
                {
                    response = FormatResponseFromValidationFailures(failures);
                }
                else
                {
                    response = await HandleCoreAsync(request);
                }
            }
            catch (ValidationFailedException e)
            {
                response = new TResponse {Error = e.Error};
            }
            catch (ExternalServiceFailedException e)
            {
                response = new TResponse { Error = e.Error };
                response.Error.Reason = ErrorReasons.ExternalServerError;
            }
            catch (Exception ex)
            {
                response = FormatGeneralError(ex);
                Logger.Error($"Error in handler: {ex.Message} \r\n Request: {JsonConvert.SerializeObject(request)} \r\n Response: {JsonConvert.SerializeObject(response)}", ex);
            }

            return response;
        }

        private TResponse FormatGeneralError(Exception ex)
        {
            var response = new TResponse();

            response.Error = new InternalServerErrorResponse(ex.Message);

            return response;
        }

        private TResponse FormatResponseFromValidationFailures(IList<ValidationFailure> failures)
        {
            Dictionary<string, List<Error>> groupedFailures = failures
                .GroupBy(failure => failure.Reason, FailureError)
				.ToDictionary(group => ValidationErrorReasons.ValidationFailed, group => group.ToList());

            var response = new TResponse();           
            response.Error = new ValidationFailedResponse(groupedFailures, "Read Params for more details");
            
            return response;
        }

        private static Error FailureError(ValidationFailure failure)
        {
            return new Error
            {
                Reason = failure.Reason,
                Message = failure.Message != null ? "Validation failure: " + failure.Message : "Validation failure"
            };
        }

        private async Task<List<ValidationFailure>> ValidateRequestWrapperAsync(TRequest request)
        {
            var errors = new List<ValidationFailure>();

            if (request == null)
            {
                Logger.Info("Empty Request received Class: " + GetType().Name);
				errors.AddError(ValidationErrorReasons.EmptyRequest, "Request is empty");
                return errors;
            }
            
            Logger.Info(request.GetType() + " Request received Class: " + GetType().Name);
            
            errors = await ValidateRequestAsync(request);

            return errors;
        }

        protected abstract Task<List<ValidationFailure>> ValidateRequestAsync(TRequest request);

        protected abstract Task<TResponse> HandleCoreAsync(TRequest request);      
    }
}
