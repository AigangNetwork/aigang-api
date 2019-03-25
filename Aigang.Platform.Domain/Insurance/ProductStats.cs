namespace Aigang.Platform.Domain.Insurance
{
    public class ProductStats
    {
        public bool Paused { get; set; }
        
        public int PoliciesLength { get; set; }

        public decimal ProductTokensPool { get; set; }
    }
}