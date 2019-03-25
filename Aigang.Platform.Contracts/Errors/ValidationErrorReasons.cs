namespace Aigang.Platform.Contracts.Errors
{
    public class ValidationErrorReasons
    {
        // Validation 4**
        public const string EmptyRequest = nameof(EmptyRequest);
        public const string InvalidQuery = nameof(InvalidQuery);
        public const string BadRequest = nameof(BadRequest);  // 400
        public const string ValidationFailed = nameof(ValidationFailed); //400
        public const string UserNotFound = nameof(UserNotFound);
       
        public const string ProductNotActive = nameof(ProductNotActive);
        public const string PoliciesLimitReached = nameof(PoliciesLimitReached);
        public const string ProductPoolLimitReached = nameof(ProductPoolLimitReached);
        
        // 4**
        public const string Unauthorized = nameof(Unauthorized); //401
        public const string Forbidden = nameof(Forbidden); //403


        // Register validation 4**
    }
}
