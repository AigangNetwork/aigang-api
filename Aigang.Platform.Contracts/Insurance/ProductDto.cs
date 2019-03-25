using System;

namespace Aigang.Platform.Contracts.Insurance
{
    public class ProductDto
    {
        public string Id { get; set; }    

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal BasePremium { get; set; }
        
        public DateTime EndDateUtc { get; set; }
        
        public DateTime CreatedUtc { get; set; }
        
        public DateTime StartDateUtc { get; set; }
         
        public string TermsAndConditions { get; set; }
        
        public string ContractAddress { get; set; }
        
        public ProductStateDto State { get; set; }

        public string ProductType { get; set; }

        public bool IsPoliciesLimitReached { get; set; }
        
        public bool IsPoolLimitReached { get; set; }
    }
}