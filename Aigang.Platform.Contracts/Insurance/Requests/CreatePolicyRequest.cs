namespace Aigang.Platform.Contracts.Insurance.Requests
{
    public class CreatePolicyRequest : BaseRequest
    {
        public string ProductAddress { get; set; }
        
        public int ProductTypeId { get; set; }
        
        public string DeviceId { get; set; }
        
        public string TaskId { get; set; }
    }
}