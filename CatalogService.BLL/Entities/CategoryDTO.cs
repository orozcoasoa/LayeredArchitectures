using System.ComponentModel.DataAnnotations;

namespace CatalogService.BLL.Entities
{
    /// <summary>
    /// Category DTO for add/update.
    /// </summary>
    public class CategoryDTO
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public string? Image { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
