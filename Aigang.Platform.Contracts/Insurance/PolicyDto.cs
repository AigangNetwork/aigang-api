using System;

namespace Aigang.Platform.Contracts.Insurance
{
    public class PolicyDto
    {
        public string Id { get; set; }

        public string DeviceId { get; set; }
        
        public DateTime? StartUtc { get; set; }

        public DateTime? EndUtc { get; set; }

        public DateTime ModifiedUtc { get; set; }

        public PolicyStatusDto Status { get; set; }

        public decimal Premium { get; set; }

        public decimal Fee { get; set; }

        public decimal Payout { get; set; }

        public DateTime PayoutUtc { get; set; }
    
        public string Properties { get; set; }

        public string ClaimProperties { get; set; }

        public string ProductAddress { get; set; }
        
        public ProductTypeDto ProductTypeId { get; set; }

        public DateTime CreateUtc { get; set; }
        
        public bool IsCanceled { get; set; }
    }
}