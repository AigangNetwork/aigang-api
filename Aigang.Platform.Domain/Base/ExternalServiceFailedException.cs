using System;
using Aigang.Platform.Contracts.Errors;

namespace Aigang.Platform.Domain.Base
{
    public class ExternalServiceFailedException : Exception
    {     
        public ErrorResponse Error { get; set; }

        public ExternalServiceFailedException(ErrorResponse error)
        {
            Error = error;
        }
    }
}