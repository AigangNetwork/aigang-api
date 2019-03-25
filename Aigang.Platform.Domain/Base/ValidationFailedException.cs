using System;
using System.Collections.Generic;
using Aigang.Platform.Contracts.Errors;

namespace Aigang.Platform.Domain.Base
{
    public class ValidationFailedException : Exception
    {     
        public ErrorResponse Error { get; set; }

        public ValidationFailedException(ErrorResponse error)
        {
            Error = error;
        }

        public ValidationFailedException(string reason)
        {
            InitializeError();
            
			Error.Params[ValidationErrorReasons.ValidationFailed].Add(new Error() { Reason = reason, Message = "Validation failure" });
        }

        public ValidationFailedException(List<Error> errors)
        {
            InitializeError();

			Error.Params[ValidationErrorReasons.ValidationFailed] = errors;
        }

        private void InitializeError()
        {
			Error = new ErrorResponse() { Reason = ValidationErrorReasons.ValidationFailed, Message = "Read Params for more details" };
            Error.Params = new Dictionary<string, List<Error>>();

			Error.Params.Add(ValidationErrorReasons.ValidationFailed, new List<Error>());
        }
    }
}
