using System.ComponentModel.DataAnnotations;

namespace CatalogService.Core.DAL
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage ="Max allowed characters:{0}")]
        public string Name { get; set; }
        public string Image { get; set; }
        public Category ParentCategory { get; set; }
    }
}