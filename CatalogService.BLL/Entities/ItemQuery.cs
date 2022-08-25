using Microsoft.AspNetCore.Mvc;

namespace CatalogService.BLL.Entities
{
    public class ItemQuery
    {
        private const int DEFAULT_PAGE_NUMBER = 1;
        private const int DEFAULT_PAGE_SIZE = 10;

        [FromQuery(Name = "page")]
        public int Page { get; set; } = DEFAULT_PAGE_NUMBER;
        [FromQuery(Name = "limit")]
        public int Limit { get; set; } = DEFAULT_PAGE_SIZE;
        [FromQuery(Name = "categoryid")]
        public int CategoryId { get; set; }
        [FromQuery(Name = "pricemin")]
        public decimal? PriceMin { get; set; }
        [FromQuery(Name = "pricemax")]
        public decimal? PriceMax { get; set; }
    }
}
