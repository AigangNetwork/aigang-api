using System.Collections.Generic;

namespace Aigang.Platform.Contracts.Errors
{
    public class ValidationFailedResponse : ErrorResponse
    {
        public ValidationFailedResponse(Dictionary<string, List<Error>> additionalParams, string message = default(string))
        {
			Reason = ValidationErrorReasons.ValidationFailed;
            Params = additionalParams;
            Message = message;
        }

        public ValidationFailedResponse(string message)
        {
			Reason = ValidationErrorReasons.ValidationFailed;
            Message = message;
        }
    }
}
