namespace CatalogService.BLL.Entities
{
    public class ItemDetails
    {
        public int ItemId { get; set; }
        public Dictionary<string, string> Details { get; set; } = new Dictionary<string, string>();
    }
}
