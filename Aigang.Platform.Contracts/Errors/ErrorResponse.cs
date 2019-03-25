using System.Collections.Generic;

namespace Aigang.Platform.Contracts.Errors
{
    public interface IErrorResponse
    {
        string Reason { get; }
        string Message { get; }
        Dictionary<string, List<Error>> Params { get; }
    }

    public class ErrorResponse : IErrorResponse
    {
        public ErrorResponse() { }

        public ErrorResponse(string reason)
        {
            Reason = reason;
        }

        public ErrorResponse(string reason, string message)
        {
            Reason = reason;
            Message = message;
        }

        public ErrorResponse(string reason, string message, Dictionary<string, List<Error>> additionalParams)
        {
            {
                Reason = reason;
                Message = message;
                Params = additionalParams;
            }
        }

        public string Reason { get; set; }
        public string Message { get; set; }
        public Dictionary<string, List<Error>> Params { get; set; }
    }
}
