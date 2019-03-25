namespace Aigang.Platform.Contracts.Errors
{
    public class InternalServerErrorResponse : ErrorResponse
    {
        public InternalServerErrorResponse()
        {
            Reason = ErrorReasons.InternalServerError;
        }

        public InternalServerErrorResponse(string message)
        {
            Reason = ErrorReasons.InternalServerError;
            Message = message;
        }
    }
}
