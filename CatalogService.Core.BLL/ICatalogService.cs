using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Core.BLL
{
    public interface ICatalogService
    {
        #region Category
        Task<Category> AddCategoryAsync(string name, string image, int? pareparentCategoryIdntCategory);
        Task<Category> AddCategoryAsync(Category category);
        Task<List<Category>> GetAllCategoriesAsync();
        Task<List<Category>> GetCategoriesAsync(int parentCategoryId);
        Task<Category> GetCategoryAsync(int id);
        Task<Category> GetCategoryAsync(string name);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(int id);
        #endregion

        #region Item
        Task<Item> AddItemAsync(Item item);
        Task<List<Item>> GetAllItemsAsync();
        Task<List<Item>> GetItemsAsync(int categoryId);
        Task<List<Item>> GetItemsAsync(double priceMin, double priceMax);
        Task<Item> GetItemAsync(string name);
        Task<Item> GetItemAsync(int id);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(int id);
        #endregion

    }
}
