namespace Aigang.Platform.Contracts.Errors
{
    public class NotFoundResponse : ErrorResponse
    {
        public NotFoundResponse()
        {
			Reason = ErrorReasons.NotFound;
        }
        public NotFoundResponse(string message)
        {
			Reason = ErrorReasons.NotFound;
            Message = message;
        }
    }
}
