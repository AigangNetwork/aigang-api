using Aigang.Platform.Contracts.Insurance;
using Aigang.Platform.Domain.Insurance;
using AutoMapper;

namespace Aigang.Platform.Handlers.Utils
{
    public static class MapperConfiguration
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Policy, PolicyDto>();
                cfg.CreateMap<PolicyStatus, PolicyStatusDto>();
                cfg.CreateMap<ProductType, ProductTypeDto>();   
            });
        }
    }
}