using System.Net;
using Aigang.Platform.API.Utils;
using Aigang.Platform.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Aigang.Platform.API.Controllers
{
    public class ControllerBase : Controller
    {
        protected ActionResult MakeActionResult(BaseResponse response)
        {
            if (response.Error != null)
            {
                var errorResponse = new ObjectResult(response.Error);
                errorResponse.StatusCode = (int)HttpStatusResolver.ResolveStatusCodeFromErrors(response.Error);

                return errorResponse;
            }

            if (response.SuccessStatusCode == HttpStatusCode.Accepted)
            {
                return Accepted(response);
            }
    
            return Ok(response);
        }
    }
}
