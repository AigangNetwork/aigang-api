using System.Net;
using System.Net.Http;
using Aigang.Platform.Contracts.Errors;
using Aigang.Platform.Domain.Base;

namespace Aigang.Platform.Utils
{
    public static class HttpClientService
    {
        private static readonly HttpClient _client = new HttpClient();

        public static HttpClient GetHttpClient()
        {
            return _client;
        }

        public static void ThrowHandledException(HttpStatusCode statusCode, string errorMessage)
        {
            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new ValidationFailedException(new ErrorResponse(ErrorReasons.NotFound, errorMessage));
                case HttpStatusCode.BadRequest:
                    throw new ValidationFailedException(new ErrorResponse(ValidationErrorReasons.BadRequest, errorMessage));
                default: //HttpStatusCode.InternalServerError
                    throw new ExternalServiceFailedException(new ErrorResponse(ErrorReasons.ExternalServerError, errorMessage));
            }
        }

        public static void ThrowHandledException(HttpStatusCode statusCode, string errorMessage, string reason)
        {
            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    throw new ValidationFailedException(new ErrorResponse(reason, errorMessage));
                case HttpStatusCode.BadRequest:
                    throw new ValidationFailedException(new ErrorResponse(reason, errorMessage));
                default:
                    throw new ExternalServiceFailedException(new ErrorResponse(reason, errorMessage));
            }
        }
    }
}