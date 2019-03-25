using Aigang.Platform.Contracts.Errors;
using System;
using System.Net;
using Aigang.Platform.Domain.Base;

namespace Aigang.Platform.API.Utils
{
    public static class HttpStatusResolver
    {
        public static HttpStatusCode ResolveStatusCodeFromErrors(ErrorResponse errorResponse)
        {
            if (errorResponse?.Reason != null)
            {
                if (ErrorReasonsChecker.IsValidationError(errorResponse.Reason))
                {
                    return HttpStatusCode.BadRequest;
                }

                if (ErrorReasonsChecker.IsTimeout(errorResponse.Reason))
                {
                    return HttpStatusCode.RequestTimeout;
                }

                if (ErrorReasonsChecker.IsGeneralError(errorResponse.Reason) || ErrorReasonsChecker.IsStorageError(errorResponse.Reason))
                {
                    return HttpStatusCode.InternalServerError;
                }

                if (ErrorReasonsChecker.IsNotFound(errorResponse.Reason))
                {
                    return HttpStatusCode.NotFound;
                }

                if (ErrorReasonsChecker.IsUnauthorized(errorResponse.Reason))
                {
                    return HttpStatusCode.Unauthorized;
                }

                if(ErrorReasonsChecker.IsForbidden(errorResponse.Reason))
                {
                    return HttpStatusCode.Forbidden;
                }

                if (ErrorReasonsChecker.IsExternalServerError(errorResponse.Reason))
                {
                    return HttpStatusCode.ServiceUnavailable;
                }
                

                throw new Exception("Error collection contains mixed types of errors");
            }

            return HttpStatusCode.OK;
        }
    }
}
