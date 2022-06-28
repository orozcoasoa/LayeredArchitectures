using CatalogService.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.BLL
{
    public interface ICatalogService
    {
        #region Category
        Task<Category> AddCategory(CategoryDTO categoryDTO);
        Task<List<Category>> GetAllCategories();
        Task<List<Category>> GetCategories(int parentCategoryId);
        Task<Category> GetCategory(int id);
        Task<Category> GetCategory(string name);
        Task UpdateCategory(int id, CategoryDTO category);
        Task DeleteCategory(int id);
        #endregion

        #region Item
        Task<Item> AddItem(ItemDTO itemDTO);
        Task<List<Item>> GetAllItems();
        Task<IPagedCollection<Item>> GetItems(ItemQuery itemQuery);
        Task<Item> GetItem(string name);
        Task<Item> GetItem(int id);
        Task UpdateItem(int id, ItemDTO itemDTO);
        Task DeleteItem(int id);
        #endregion

    }
}
