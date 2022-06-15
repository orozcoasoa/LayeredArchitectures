using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Core.BLL
{
    public interface ICatalogService
    {

        Task<Category> AddCategoryAsync(string name, string image, int? pareparentCategoryIdntCategory);
        Task<Category> AddCategoryAsync(Category category);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<Category>> GetCategoriesAsync(int parentCategory);
        Task<Category> GetCategoryAsync(int id);
        Task<Category> GetCategoryAsync(string name);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
    }
}
