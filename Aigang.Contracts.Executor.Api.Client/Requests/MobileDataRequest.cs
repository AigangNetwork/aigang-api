using Aigang.Contracts.Executor.Api.Client.DTO;

namespace Aigang.Contracts.Executor.Api.Client.Requests
{
    public class MobileDataRequest
    {
        public int ProductTypeId { get; set; }
        
        public string ProductAddress { get; set; }
        
        public MobileDataDto MobileData { get; set; }
    }
}