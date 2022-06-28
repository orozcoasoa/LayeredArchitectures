using AutoMapper;
using CatalogService;
using CatalogService.BLL.Entities;
using CatalogService.BLL.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.BLL
{
    public class CatalogEFService : ICatalogService
    {
        private readonly DAL.CatalogServiceDbContext _context;
        private readonly IMapper _mapper;

        public CatalogEFService(DAL.CatalogServiceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Category
        public async Task<Category> AddCategory(CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            await ValidateCategory(category);
            var categoryDAO = _mapper.Map<DAL.Category>(category);
            categoryDAO.Id = 0;
            if (categoryDAO.ParentCategoryId == 0 &&
                !string.IsNullOrEmpty(category.ParentCategory?.Name))
            {
                var parentCategory = await GetCategory(category.ParentCategory.Name);
                if (parentCategory != null)
                    categoryDAO.ParentCategoryId = parentCategory.Id;
            }
            _context.Add(categoryDAO);
            await _context.SaveChangesAsync();
            return _mapper.Map<Category>(categoryDAO);
        }
        public async Task DeleteCategory(int id)
        {
            var category = await _context.Categories
                            .Where(c => c.Id == id)
                            .FirstOrDefaultAsync();
            if (category != null)
            {
                var items = await _context.Items
                        .Where(i => i.CategoryId == id)
                        .ToListAsync();
                if (items.Any())
                    _context.Remove(items);
                _context.Remove(category);

                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Category>> GetAllCategories()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<List<DAL.Category>, List<Category>>(categories);
        }
        public async Task<List<Category>> GetCategories(int parentCategoryId)
        {
            var categories = await _context.Categories
                            .Where(c => c.ParentCategory.Id == parentCategoryId)
                            .ToListAsync();
            return _mapper.Map<List<DAL.Category>, List<Category>>(categories);
        }
        public async Task<Category> GetCategory(int id)
        {
            var category = await _context.Categories
                            .Where(c => c.Id == id)
                            .FirstOrDefaultAsync();
            return _mapper.Map<Category>(category);
        }
        public async Task<Category> GetCategory(string name)
        {
            var category = await _context.Categories
                            .Where(c => c.Name == name)
                            .FirstOrDefaultAsync();
            return _mapper.Map<Category>(category);
        }
        public async Task UpdateCategory(int id, CategoryDTO categoryDTO)
        {
            var category = _mapper.Map<Category>(categoryDTO);
            category.Id = id;
            ValidateParentCategory(category);
            var categoryDAO = await _context.Categories
                                    .Where(c => c.Id == id)
                                    .Include(c => c.ParentCategory)
                                    .FirstOrDefaultAsync();
            if (categoryDAO == null)
            {
                //TODO: custom exception
                throw new KeyNotFoundException("Category " + id + " not found.");
            }
            _mapper.Map(category, categoryDAO);
            if (category.ParentCategory == null)
                categoryDAO.ParentCategory = null;
            await _context.SaveChangesAsync();
        }

        private async Task ValidateCategory(Category category)
        {
            await ValidateCategoryName(category.Name);
            ValidateParentCategory(category);
        }
        private async Task ValidateCategoryName(string name)
        {
            if (name?.Length > 50)
            {
                //TODO: create custom exception.
                throw new ArgumentException("Name", "Max length is 50");
            }
            var existingCategory = await GetCategory(name);
            if (existingCategory != null)
            {
                //TODO: create custom exception.
                throw new ArgumentException("Name", "Name already exists");
            }
        }
        private void ValidateParentCategory(Category category)
        {
            if (category.ParentCategory != null)
            {
                if (category.ParentCategory.Id == 0 &&
                    string.IsNullOrEmpty(category.ParentCategory.Name))
                {
                    //TODO: create custom exception.
                    throw new ArgumentException("ParentCategory", "Id should be valid for Parent Category");
                }
            }
            if (category.ParentCategory?.Name == category.Name ||
                (category.ParentCategory?.Id == category.Id && category.Id > 0))
            {
                //TODO: create custom exception.
                throw new ArgumentException("ParentCategory", "Circular reference.");
            }
        }
        #endregion

        #region Item
        public async Task<Item> AddItem(ItemDTO itemDTO)
        {
            var item = _mapper.Map<Item>(itemDTO);
            await ValidateItem(item);
            var itemDAO = _mapper.Map<DAL.Item>(item);
            itemDAO.Id = 0;
            _context.Add(itemDAO);
            await _context.SaveChangesAsync();
            return _mapper.Map<Item>(itemDAO);
        }
        public async Task<List<Item>> GetAllItems() {
            var items = await _context.Items.ToListAsync();
            return _mapper.Map<List<Item>>(items);
        }
        public async Task<IPagedCollection<Item>> GetItems(ItemQuery itemQuery)
        {
            var filter = GetItemFilter(itemQuery);
            var itemsDAL = await _context.Items
                        //.Where(i => i.CategoryId == itemQuery.CategoryId)
                        .Where(filter)
                        .ToPagedCollectionAsync(itemQuery.Page, itemQuery.Limit);

            var itemsList = _mapper.Map<IPagedCollection<DAL.Item>, IReadOnlyList<Item>>(itemsDAL);

            return new PagedCollection<Item>(itemsList, itemsDAL.ItemCount, itemsDAL.CurrentPageNumber, itemsDAL.PageSize);
        }
        public async Task<Item> GetItem(string name)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Name == name);
            return _mapper.Map<Item>(item);
        }
        public async Task<Item> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            return _mapper.Map<Item>(item);
        }
        public async Task UpdateItem(int id, ItemDTO itemDTO)
        {
            var item = _mapper.Map<Item>(itemDTO);
            item.Id = id;
            ValidateItemNumbers(item);
            await ValidateItemCategory(item);
            var itemDAO = await _context.Items.FindAsync(item.Id);
            if (itemDAO == null)
            {
                //TODO: custom exception
                throw new KeyNotFoundException("Item " + id + " not found.");
            }
            _mapper.Map(item, itemDAO);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteItem(int id)
        {
            var item = await _context.Items
                            .Where(i => i.Id == id)
                            .FirstOrDefaultAsync();
            if(item != null)
            {
                _context.Items.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        private async Task ValidateItem(Item item)
        {
            await ValidateItemName(item.Name);
            ValidateItemNumbers(item);
            await ValidateItemCategory(item);
        }
        private async Task ValidateItemName(string name)
        {
            if(name?.Length > 50)
            {
                //TODO: create custom exception.
                throw new ArgumentException("Name", "Max length is 50");
            }

            var existingItem = await GetItem(name);
            if (existingItem != null)
            {
                //TODO: create custom exception.
                throw new ArgumentException("Name", "Name already exists");
            }
        }
        private async Task ValidateItemCategory(Item item)
        {
            if (item.Category == null)
            {
                throw new ArgumentException("Item.Category", "Item must have category.");
            }
            if (item.Category.Id == 0 &&
                string.IsNullOrEmpty(item.Category.Name))
            {
                //TODO: create custom exception.
                throw new ArgumentException("Item.Category", "Id or name should be valid for Parent Category");
            }
            var existingCategory = await _context.Categories
                                            .Where(c => c.Id == item.Category.Id || c.Name == item.Category.Name)
                                            .FirstOrDefaultAsync();
            if (existingCategory == null)
            {
                throw new ArgumentException("Item.Category", "Item category not found.");
            }
        }
        private void ValidateItemNumbers(Item item)
        {
            if (!(item.Price >= 0))
            {
                throw new ArgumentException("Price", "Price must be positive");
            }
            if (!(item.Amount >= 0))
            {
                throw new ArgumentException("Amount", "Amount must be positive");
            }
        }
        private Expression<Func<DAL.Item,bool>> GetItemFilter(ItemQuery itemQuery)
        {
            var filterConstant = Expression.Constant(true);
            Expression<Func<DAL.Item, bool>> filter = i => i.CategoryId == itemQuery.CategoryId;
            if (itemQuery.PriceMax.HasValue)
            {
                Expression<Func<DAL.Item, bool>> priceMaxFilter = i => i.Price <= itemQuery.PriceMax;
                var invokedExpr = Expression.Invoke(priceMaxFilter,filter.Parameters.Cast<Expression>());

                filter = Expression.Lambda<Func<DAL.Item, bool>>(
                            Expression.AndAlso(filter.Body, invokedExpr), filter.Parameters);
            }
            if (itemQuery.PriceMin.HasValue)
            {
                Expression<Func<DAL.Item, bool>> priceMinFilter = i => i.Price >= itemQuery.PriceMin;
                var invokedExpr = Expression.Invoke(priceMinFilter, filter.Parameters.Cast<Expression>());

                filter = Expression.Lambda<Func<DAL.Item, bool>>(
                            Expression.AndAlso(filter.Body, invokedExpr), filter.Parameters);
            }

            return filter;

        }
        #endregion
    }
}
