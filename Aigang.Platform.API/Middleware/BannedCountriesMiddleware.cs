using System.Collections.Generic;
using System.Threading.Tasks;
using Aigang.Platform.Utils;
using log4net;
using Microsoft.AspNetCore.Http;

namespace Aigang.Platform.API.Middleware
{
    public class BannedCountriesMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(BannedCountriesMiddleware));
        private readonly List<string> _bannedCountries = ConfigurationManager.GetStringList("BannedCountries");
        private readonly bool _isBannedCountriesEnabled = ConfigurationManager.GetBool("BannedCountriesEnabled");
        
        public BannedCountriesMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!_isBannedCountriesEnabled)
            {
                await _next.Invoke(context);
                return;
            }
            
            var ipCountry = context.Request.Headers["CF-IPCountry"];

            if (_bannedCountries.Contains(ipCountry)) // empty strings matching too.
            {
                _logger.Info($"Access not allowed for user in country: {ipCountry}. Request: {context.Request.Method} {context.Request.Path}");
                context.Response.StatusCode = 406;
            }
            else
            {
                await _next.Invoke(context);
            }
        }
    }
}