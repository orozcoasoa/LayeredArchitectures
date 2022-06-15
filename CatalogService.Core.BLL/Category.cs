namespace CatalogService.Core.BLL
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public Category ParentCategory { get; set; }
    }
}