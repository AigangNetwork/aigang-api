namespace Aigang.Contracts.Executor.Api.Client.Requests
{
    public class PolicyClaimRequest
    {
        public string ProductAddress { get; set; }
        public string PolicyId { get; set; }
        public string ClaimProperties { get; set; }
    }
}