using AutoMapper;
using CatalogService.Core.BLL;
using CatalogService.Core.DAL;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.UnitTests
{
    public class CatalogEFServiceCategoryTest
    {
        private static IMapper _mapper;
        private readonly CatalogServiceDbContext _context;

        public CatalogEFServiceCategoryTest()
        {
            var contextOptions = new DbContextOptionsBuilder<CatalogServiceDbContext>()
                                .UseInMemoryDatabase("CatalogServiceTest")
                                .Options;
            _context = new CatalogServiceDbContext(contextOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.AddRange(
                    new Core.DAL.Category()
                    {
                        Id = 1,
                        Name = "Cleaning",
                        Image = "",
                    },
                    new Core.DAL.Category()
                    {
                        Id = 2,
                        Name = "Mops",
                        Image = "",
                        ParentCategoryId = 1,
                    },
                    new Core.DAL.Category()
                    {
                        Id = 3,
                        Name = "Brooms",
                        Image = "",
                        ParentCategoryId = 1,
                    },
                    new Core.DAL.Category()
                    {
                        Id = 4,
                        Name = "Buckets",
                        Image = "",
                    }
               );

            _context.SaveChanges();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new CatalogProfile()));
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public async Task GetAllCategories()
        {
            var service = new CatalogEFService(_context, _mapper);
            var categories = await service.GetAllCategoriesAsync();
            Assert.Equal(4, categories.Count);
        }
        [Fact]
        public async Task GetCategoriesByParent()
        {
            var service = new CatalogEFService(_context, _mapper);
            var categories = await service.GetCategoriesAsync(1);
            Assert.Equal(2, categories.Count);
        }
        [Fact]
        public async Task GetCategoriesByParent_Empty()
        {
            var service = new CatalogEFService(_context, _mapper);
            var categories = await service.GetCategoriesAsync(2);
            Assert.Empty(categories);
        }
        [Fact]
        public async Task GetCategoriesByParent_NonExistentId()
        {
            var service = new CatalogEFService(_context, _mapper);
            var categories = await service.GetCategoriesAsync(5);
            Assert.Empty(categories);
        }
        [Fact]
        public async Task AddCategoryByFields()
        {
            var service = new CatalogEFService(_context, _mapper);
            var categoryAdded = await service.AddCategoryAsync("Rags", "", null);
            Assert.NotNull(categoryAdded);
            Assert.True(categoryAdded.Id > 0);
        }
        [Fact]
        public async Task AddCategoryByFields_WrongParentId()
        {
            var service = new CatalogEFService(_context, _mapper);
            var categoryAdded = await service.AddCategoryAsync("Rags", "", 10);
            Assert.NotNull(categoryAdded);
            Assert.True(categoryAdded.Id > 0);
            Assert.Null(categoryAdded.ParentCategory);
        }
        [Fact]
        public async Task AddCategoryByFields_WithParentId()
        {
            var service = new CatalogEFService(_context, _mapper);
            var categoryAdded = await service.AddCategoryAsync("Rags", "", 1);
            Assert.NotNull(categoryAdded);
            Assert.True(categoryAdded.Id > 0);
            Assert.NotNull(categoryAdded.ParentCategory);
        }
        [Fact]
        public async Task AddCategoryByFields_RepeatedName()
        {
            var service = new CatalogEFService(_context, _mapper);
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddCategoryAsync("Mops", "", null));
        }
        [Fact]
        public async Task AddCategory()
        {
            var service = new CatalogEFService(_context, _mapper);
            var newCategory = new Core.BLL.Category() { Name = "Rags", Image = "" };
            var categoryAdded = await service.AddCategoryAsync(newCategory);
            Assert.NotNull(categoryAdded);
            Assert.True(categoryAdded.Id > 0);
            Assert.Equal(newCategory.Name, categoryAdded.Name);
            Assert.Null(categoryAdded.ParentCategory);
        }
        [Fact]
        public async Task AddCategory_WithId()
        {
            var service = new CatalogEFService(_context, _mapper);
            var newCategory = new Core.BLL.Category() {Id=10, Name = "Rags", Image = "" };
            var categoryAdded = await service.AddCategoryAsync(newCategory);
            Assert.NotNull(categoryAdded);
            Assert.True(categoryAdded.Id > 0);
            Assert.Equal(newCategory.Name, categoryAdded.Name);
            Assert.Null(categoryAdded.ParentCategory);
            Assert.NotEqual(newCategory.Id, categoryAdded.Id);
        }
        [Fact]
        public async Task AddCategory_WithParentCategory()
        {
            var service = new CatalogEFService(_context, _mapper);
            var parentCategory = new Core.BLL.Category() { Id = 1 };
            var newCategory = new Core.BLL.Category() { Name = "Rags", Image = "", ParentCategory = parentCategory };
            var categoryAdded = await service.AddCategoryAsync(newCategory);
            Assert.NotNull(categoryAdded);
            Assert.True(categoryAdded.Id > 0);
            Assert.Equal(newCategory.Name, categoryAdded.Name);
            Assert.NotNull(categoryAdded.ParentCategory);

        }
        [Fact]
        public async Task AddCategory_WithParentCategoryName()
        {
            var service = new CatalogEFService(_context, _mapper);
            var parentCategory = new Core.BLL.Category() { Name="Cleaning" };
            var newCategory = new Core.BLL.Category() { Name = "Rags", Image = "", ParentCategory = parentCategory };
            var categoryAdded = await service.AddCategoryAsync(newCategory);
            Assert.NotNull(categoryAdded);
            Assert.True(categoryAdded.Id > 0);
            Assert.Equal(newCategory.Name, categoryAdded.Name);
            Assert.NotNull(categoryAdded.ParentCategory);

        }
        [Fact]
        public async Task AddCategory_WithParentCategoryInvalid()
        {
            var service = new CatalogEFService(_context, _mapper);
            var parentCategory = new Core.BLL.Category() {};
            var newCategory = new Core.BLL.Category() { Name = "Rags", Image = "", ParentCategory = parentCategory };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddCategoryAsync(newCategory));
        }
        [Fact]
        public async Task AddCategory_RepeatedName()
        {
            var service = new CatalogEFService(_context, _mapper);
            var newCategory = new Core.BLL.Category() { Name = "Mops" };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddCategoryAsync(newCategory));
        }
        [Fact]
        public async Task DeleteCategory()
        {
            var service = new CatalogEFService(_context, _mapper);
            await service.DeleteCategoryAsync(2);
            var categoriesNum = _context.Categories.Count();
            Assert.Equal(3, categoriesNum);
        }
        [Fact]
        public async Task DeleteCategory_CategoryParent()
        {
            var service = new CatalogEFService(_context, _mapper);
            await service.DeleteCategoryAsync(1);
            var categoriesNum = _context.Categories.Count();
            Assert.Equal(3, categoriesNum);
        }
        [Fact]
        public async Task DeleteCategory_NonExistentId()
        {
            var service = new CatalogEFService(_context, _mapper);
            await service.DeleteCategoryAsync(99);
            var categoriesNum = _context.Categories.Count();
            Assert.Equal(4, categoriesNum);
        }
        [Fact]
        public async Task UpdateCategory()
        {
            var service = new CatalogEFService(_context, _mapper);
            var categoryUpdated = new Core.BLL.Category()
            {
                Id = 4,
                Name = "Buckets",
                Image = "URL",
            };
            await service.UpdateCategoryAsync(categoryUpdated);
            var updatedCategory = await _context.Categories.FindAsync(4);
            Assert.NotNull(updatedCategory);
            Assert.Equal(updatedCategory.Image, categoryUpdated.Image);
        }
        [Fact]
        public async Task UpdateCategory_ParentCategory()
        {
            var service = new CatalogEFService(_context, _mapper);
            var categoryUpdated = new Core.BLL.Category()
            {
                Id = 4,
                Name = "Buckets",
                Image = "",
                ParentCategory = new Core.BLL.Category() { Id=1}
            };
            await service.UpdateCategoryAsync(categoryUpdated);
            var updatedCategory = await _context.Categories.FindAsync(4);
            Assert.NotNull(updatedCategory);
            Assert.NotNull(updatedCategory.ParentCategory);
        }
        [Fact]
        public async Task UpdateCategory_WrongId()
        {
            var service = new CatalogEFService(_context, _mapper);
            var categoryUpdated = new Core.BLL.Category()
            {
                Id = 10,
                Name = "Buckets",
                Image = "",
            };
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateCategoryAsync(categoryUpdated));
        }
        [Fact]
        public async Task UpdateCategory_WrongParent()
        {
            var service = new CatalogEFService(_context, _mapper);
            var categoryUpdated = new Core.BLL.Category()
            {
                Id = 4,
                Name = "Buckets",
                Image = "",
                ParentCategory = new Core.BLL.Category() {}
            };
            await Assert.ThrowsAsync<ArgumentException>(() => service.UpdateCategoryAsync(categoryUpdated));
        }
    }
}