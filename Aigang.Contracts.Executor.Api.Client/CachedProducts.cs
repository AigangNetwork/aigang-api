using System;
using System.Threading.Tasks;
using Aigang.Platform.Domain.Insurance;
using Microsoft.Extensions.Caching.Memory;

namespace Aigang.Contracts.Executor.Api.Client
{
    public static class CachedProducts
    {
        private static TimeSpan _expirationTime = TimeSpan.FromHours(24);

        private static IMemoryCache _cache = new MemoryCache(new MemoryCacheOptions
        {
            ExpirationScanFrequency = TimeSpan.FromMinutes(30)
        });
        
        public static async Task<Product> GetProductAsync(int productTypeId,string productAddress)
        {
            if (_cache.TryGetValue(productAddress, out Product result))
            {
                return result;
            }

            Product response = await ContractsExecutorClient.GetProductDetails(productAddress, productTypeId);

            if (response.BasePremium > 0)
            {
                AddValue(productAddress, response);
                return response;
            }
            
            return null;
        }

        private static void AddValue(string address, Product product)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions();
 
            cacheEntryOptions.SetAbsoluteExpiration(_expirationTime);
        
            _cache.Set(address, product, cacheEntryOptions);
        }

    }
}