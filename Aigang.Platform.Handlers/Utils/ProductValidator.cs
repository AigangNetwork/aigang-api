using System;
using Aigang.Platform.Domain.Insurance;

namespace Aigang.Platform.Handlers.Utils
{
    public static class ProductValidator
    {

        public static bool IsProductActive(Product product)
        {
            var now = DateTime.UtcNow;
            
            // product should be started
            if (!(product.StartDateUtc <= now && product.EndDateUtc >= now))
            {
                return false;
            }
            
            // TODO: add all other validation form contract
            
            return true;
        }
        
    }
}