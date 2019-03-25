namespace Aigang.Platform.Contracts.Insurance.Responses
{
    public class VerifyPolicyClaimResponse : BaseResponse
    {
        public bool IsClaimable { get; set; }

        public bool IsTaskFinished { get; set; }
    }
}