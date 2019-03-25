using System.Collections.Generic;

namespace Aigang.Platform.Contracts.Insurance.Responses
{
    public class GetProductListResponse : BaseResponse
    {
        public IEnumerable<ProductListItemDto> Items { get; set; }
        
        public int TotalPages { get; set; }
    }
}