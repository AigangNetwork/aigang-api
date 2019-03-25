namespace Aigang.Platform.Contracts.Errors
{
    public static class ErrorReasons
    {      
        // Server Errors 5**
        public const string InternalServerError = nameof(InternalServerError); // 500
        public const string StorageErrorException = nameof(StorageErrorException); // 500
        
        // External service errors
        public const string ExternalServerError = nameof(ExternalServerError); // 503
        
        public const string NotFound = nameof(NotFound); // 404
        public const string GatewayTimeout = nameof(GatewayTimeout);  //408
    }
}
