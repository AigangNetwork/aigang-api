using System.Net;
using Aigang.Platform.Contracts.Errors;

namespace Aigang.Platform.Contracts
{
    public class BaseResponse
    {
        public ErrorResponse Error { get; set; }

        public HttpStatusCode? SuccessStatusCode { get; set; }
    }
}
