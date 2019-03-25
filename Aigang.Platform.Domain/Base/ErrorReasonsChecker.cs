using Aigang.Platform.Contracts.Errors;

namespace Aigang.Platform.Domain.Base
{
    public static class ErrorReasonsChecker
    {
        public static bool IsValidationError(string errorReasons)
        {
            return errorReasons != null
                && !IsTimeout(errorReasons)
                && !IsNotFound(errorReasons)
                && !IsUnauthorized(errorReasons)
                && !IsGeneralError(errorReasons)
                && !IsStorageError(errorReasons)
                && !IsForbidden(errorReasons)
                && !IsUserNotFound(errorReasons)
                && !IsExternalServerError(errorReasons);
        }

        public static bool IsForbidden(string errorReasons)
        {
			return errorReasons == ValidationErrorReasons.Forbidden;
        }

        public static bool IsGeneralError(string errorReasons)
        {
            return errorReasons == ErrorReasons.InternalServerError;
        }

        public static bool IsNotFound(string errorReasons)
        {
			return errorReasons == ErrorReasons.NotFound;
        }

        public static bool IsTimeout(string errorReasons)
        {
			return errorReasons == ErrorReasons.GatewayTimeout;
        }

        public static bool IsStorageError(string errorReasons)
        {
			return errorReasons == ErrorReasons.StorageErrorException;
        }

        public static bool IsUnauthorized(string errorReasons)
        {
			return errorReasons == ValidationErrorReasons.Unauthorized;
        }
        
        public static bool IsUserNotFound(string errorReasons)
        {
			return errorReasons == ValidationErrorReasons.UserNotFound;
        }

        public static bool IsExternalServerError(string errorReasons)
        {
            return errorReasons == ErrorReasons.ExternalServerError;
        }
    }
}
