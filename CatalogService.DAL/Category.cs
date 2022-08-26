using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.DAL
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "Max allowed characters:{0}")]
        public string Name { get; set; }
        public string Image { get; set; }
        [ForeignKey("Category")]
        public int? ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
    }
}