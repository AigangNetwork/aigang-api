namespace Aigang.Platform.Contracts.Insurance.Responses
{
    public class GetPolicyTransactionsResponse : BaseResponse
    {
        public PolicyTransactionsDto PolicyTransactions { get; set; }
    }
}