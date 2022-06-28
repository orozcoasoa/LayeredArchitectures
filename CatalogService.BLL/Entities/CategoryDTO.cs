using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
