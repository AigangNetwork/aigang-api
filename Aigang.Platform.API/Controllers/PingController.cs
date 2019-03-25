using Microsoft.AspNetCore.Mvc;

namespace Aigang.Platform.API.Controllers
{
    [Route("api/ping")]
    public class PingController : Controller
    {
        /// <summary>
        /// PING Endpoint
        /// </summary>
        [HttpGet]
        public string Ping()
        {
            return "PONG";
        }
    }
}
