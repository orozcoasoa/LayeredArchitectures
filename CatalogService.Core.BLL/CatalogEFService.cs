using AutoMapper;
using CatalogService.Core.DAL;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Core.BLL
{
    public class CatalogEFService : ICatalogService
    {
        private readonly CatalogServiceDbContext _context;
        private readonly IMapper _mapper;

        public CatalogEFService(CatalogServiceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        #region Category
        public async Task<Category> AddCategoryAsync(string name, string image, int? parentCategoryId)
        {
            DAL.Category parentCat = null;
            if (parentCategoryId != null)
            {
                parentCat = await _context.Categories.FindAsync(parentCategoryId);
            }
            await ValidateCategoryNameAsync(name);

            var categoryDAO = new DAL.Category() { Name = name, Image = image, ParentCategory = parentCat };
            _context.Categories.Add(categoryDAO);
            await _context.SaveChangesAsync();
            return _mapper.Map<Category>(categoryDAO);
        }
        public async Task<Category> AddCategoryAsync(Category category)
        {
            await ValidateCategoryAsync(category);
            var categoryDAO = _mapper.Map<DAL.Category>(category);
            categoryDAO.Id = 0;
            if (categoryDAO.ParentCategoryId == 0 && 
                !string.IsNullOrEmpty(category.ParentCategory?.Name))
            {
                var parentCategory = await GetCategoryAsync(category.ParentCategory.Name);
                if (parentCategory != null)
                    categoryDAO.ParentCategoryId = parentCategory.Id;
            }
            _context.Add(categoryDAO);
            await _context.SaveChangesAsync();
            return _mapper.Map<Category>(categoryDAO);
        }
        public async Task DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories
                            .Where(c => c.Id == id)
                            .FirstOrDefaultAsync();
            if (category != null)
            {
                _context.Remove(category);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return _mapper.Map<List<DAL.Category>, List<Category>>(categories);
        }
        public async Task<List<Category>> GetCategoriesAsync(int parentCategoryId)
        {
            var categories = await _context.Categories
                            .Where(c => c.ParentCategory.Id == parentCategoryId)
                            .ToListAsync();
            return _mapper.Map<List<DAL.Category>, List<Category>>(categories);
        }
        public async Task<Category> GetCategoryAsync(int id)
        {
            var category = await _context.Categories
                            .Where(c => c.Id == id)
                            .FirstOrDefaultAsync();
            return _mapper.Map<Category>(category);
        }
        public async Task<Category> GetCategoryAsync(string name)
        {
            var category = await _context.Categories
                            .Where(c => c.Name == name)
                            .FirstOrDefaultAsync();
            return _mapper.Map<Category>(category);
        }
        public async Task UpdateCategoryAsync(Category category)
        {
            ValidateParentCategory(category);
            var categoryDAO = await _context.Categories.FindAsync(category.Id);
            if (categoryDAO == null)
            {
                //TODO: custom exception
                throw new KeyNotFoundException();
            }
            _mapper.Map(category, categoryDAO);
            await _context.SaveChangesAsync();
        }

        private async Task ValidateCategoryAsync(Category category)
        {
            await ValidateCategoryNameAsync(category.Name);
            ValidateParentCategory(category);
        }
        private async Task ValidateCategoryNameAsync(string name)
        {
            if (name?.Length > 50)
            {
                //TODO: create custom exception.
                throw new ArgumentException("Name", "Max length is 50");
            }
            var existingCategory = await GetCategoryAsync(name);
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
        public async Task<Item> AddItemAsync(Item item)
        {
            await ValidateItemAsync(item);
            var itemDAO = _mapper.Map<DAL.Item>(item);
            itemDAO.Id = 0;
            _context.Add(itemDAO);
            _context.SaveChanges();
            return _mapper.Map<Item>(itemDAO);
        }
        public async Task<List<Item>> GetAllItemsAsync(){
            var items = await _context.Items.ToListAsync();
            return _mapper.Map<List<Item>>(items);
        }
        public async Task<List<Item>> GetItemsAsync(int categoryId)
        {
            var items = await _context.Items.Where(i => i.CategoryId==categoryId)
                                        .ToListAsync();
            return _mapper.Map<List<Item>>(items);
        }
        public async Task<List<Item>> GetItemsAsync(double priceMin, double priceMax)
        {
            var items = await _context.Items
                                .Where(i => (double)i.Price>= priceMin && (double)i.Price<=priceMax)
                                .ToListAsync();
            return _mapper.Map<List<Item>>(items);
        }
        public async Task<Item> GetItemAsync(string name)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Name == name);
            return _mapper.Map<Item>(item);
        }
        public async Task<Item> GetItemAsync(int id)
        {
            var item = await _context.Items.FindAsync(id);
            return _mapper.Map<Item>(item);
        }
        public async Task UpdateItemAsync(Item item)
        {
            ValidateItemNumbers(item);
            await ValidateItemCategoryAsync(item);
            var itemDAO = await _context.Items.FindAsync(item.Id);
            if (itemDAO == null)
            {
                //TODO: custom exception
                throw new KeyNotFoundException();
            }
            _mapper.Map(item, itemDAO);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteItemAsync(int id)
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

        private async Task ValidateItemAsync(Item item)
        {
            await ValidateItemNameAsync(item.Name);
            ValidateItemNumbers(item);
            await ValidateItemCategoryAsync(item);
        }
        private async Task ValidateItemNameAsync(string name)
        {
            if(name?.Length > 50)
            {
                //TODO: create custom exception.
                throw new ArgumentException("Name", "Max length is 50");
            }

            var existingItem = await GetItemAsync(name);
            if (existingItem != null)
            {
                //TODO: create custom exception.
                throw new ArgumentException("Name", "Name already exists");
            }
        }
        private async Task ValidateItemCategoryAsync(Item item)
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
        #endregion
    }
}
