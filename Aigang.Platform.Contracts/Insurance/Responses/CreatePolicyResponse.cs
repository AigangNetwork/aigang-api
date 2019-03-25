namespace Aigang.Platform.Contracts.Insurance.Responses
{
    public class CreatePolicyResponse : BaseResponse
    {
        public PolicyDto Policy { get; set; }

        public string ValidationResultCode { get; set; }
    }
}