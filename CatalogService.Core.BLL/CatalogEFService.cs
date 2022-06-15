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
        public async Task<List<Category>> GetCategoriesAsync(int parentCategory)
        {
            var categories = await _context.Categories
                            .Where(c => c.ParentCategory.Id == parentCategory)
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
    }
}
