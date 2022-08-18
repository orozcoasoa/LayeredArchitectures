using CatalogService.BLL.Entities;

namespace CatalogService.BLL.GraphQL
{
    public class CatalogQuery
    {

        public async Task<IList<Category>> GetCategories([Service] ICatalogService catalog) 
            => await catalog.GetAllCategories();

        [UsePaging]
        public async Task<IEnumerable<Item>> GetItemsDPaging([Service] ICatalogService catalog, int categoryId)
        {
            return await catalog.GetItems(new ItemQuery() { CategoryId = categoryId, Page = 1, Limit = int.MaxValue});
        }

        public async Task<IEnumerable<Item>> GetItemsCPaging([Service] ICatalogService catalog, int categoryId, int page, int pageSize)
        {
            return await catalog.GetItems(new ItemQuery() { CategoryId = categoryId, Page = page, Limit = pageSize });
        }
    }
}
