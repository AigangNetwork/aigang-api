namespace Aigang.Platform.Domain.Insurance
{
    public enum PolicyStatus
    {
        NotSet = 0,
        Draft = 1,
        Paid = 2,
        Claimable = 3,
        PaidOut = 4,
        Canceled = 5
        //   PendingPayout = 7,
        //   Finished = 8
    }
}