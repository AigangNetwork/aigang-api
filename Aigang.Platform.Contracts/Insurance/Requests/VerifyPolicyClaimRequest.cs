namespace Aigang.Platform.Contracts.Insurance.Requests
{
    public class VerifyPolicyClaimRequest : BaseRequest
    {
        public string TaskId { get; set; }
        
        public string PolicyId { get; set; }
        
        public string ProductAddress  { get; set; }
        
        public int ProductTypeId  { get; set; }
    }
}