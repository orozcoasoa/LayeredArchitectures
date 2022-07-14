using AutoMapper;
using CatalogService.BLL;
using CatalogService.BLL.Entities;
using CatalogService.DAL;
using MessagingService;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CatalogService.UnitTests
{
    public class CatalogEFServiceCategoryTest
    {
        private static IMapper _mapper;
        private readonly CatalogServiceDbContext _context;

        public CatalogEFServiceCategoryTest()
        {
            var contextOptions = new DbContextOptionsBuilder<CatalogServiceDbContext>()
                                .UseInMemoryDatabase("CatalogServiceCategoryTest")
                                .Options;
            _context = new CatalogServiceDbContext(contextOptions);

            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();

            _context.AddRange(
                    new DAL.Category()
                    {
                        Id = 1,
                        Name = "Cleaning",
                        Image = "",
                    },
                    new DAL.Category()
                    {
                        Id = 2,
                        Name = "Mops",
                        Image = "",
                        ParentCategoryId = 1,
                    },
                    new DAL.Category()
                    {
                        Id = 3,
                        Name = "Brooms",
                        Image = "",
                        ParentCategoryId = 1,
                    },
                    new DAL.Category()
                    {
                        Id = 4,
                        Name = "Buckets",
                        Image = "",
                    }
               );

            _context.SaveChanges();

            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc => mc.AddProfile(new BLL.Setup.CatalogProfile()));
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }
        }

        [Fact]
        public async Task GetAllCategories()
        {
            var service = GetService();
            var categories = await service.GetAllCategories();
            Assert.Equal(4, categories.Count);
        }
        [Fact]
        public async Task GetCategoriesByParent()
        {
            var service = GetService();
            var categories = await service.GetCategories(1);
            Assert.Equal(2, categories.Count);
        }
        [Fact]
        public async Task GetCategoriesByParent_Empty()
        {
            var service = GetService();
            var categories = await service.GetCategories(2);
            Assert.Empty(categories);
        }
        [Fact]
        public async Task GetCategoriesByParent_NonExistentId()
        {
            var service = GetService();
            var categories = await service.GetCategories(5);
            Assert.Empty(categories);
        }
        [Fact]
        public async Task AddCategory()
        {
            var service = GetService();
            var newCategory = new CategoryDTO() { Name = "Rags", Image = "" };
            var categoryAdded = await service.AddCategory(newCategory);
            Assert.NotNull(categoryAdded);
            Assert.True(categoryAdded.Id > 0);
            Assert.Equal(newCategory.Name, categoryAdded.Name);
            Assert.Null(categoryAdded.ParentCategory);
        }
        [Fact]
        public async Task AddCategory_WithParentCategory()
        {
            var service = GetService();
            var newCategory = new CategoryDTO() { Name = "Rags", Image = "", ParentCategoryId = 1 };
            var categoryAdded = await service.AddCategory(newCategory);
            Assert.NotNull(categoryAdded);
            Assert.True(categoryAdded.Id > 0);
            Assert.Equal(newCategory.Name, categoryAdded.Name);
            Assert.NotNull(categoryAdded.ParentCategory);

        }
        [Fact]
        public async Task AddCategory_RepeatedName()
        {
            var service = GetService();
            var newCategory = new CategoryDTO() { Name = "Mops" };
            await Assert.ThrowsAsync<ArgumentException>(() => service.AddCategory(newCategory));
        }
        [Fact]
        public async Task DeleteCategory()
        {
            var service = GetService();
            await service.DeleteCategory(2);
            var categoriesNum = _context.Categories.Count();
            Assert.Equal(3, categoriesNum);
        }
        [Fact]
        public async Task DeleteCategory_CategoryParent()
        {
            var service = GetService();
            await service.DeleteCategory(1);
            var categoriesNum = _context.Categories.Count();
            Assert.Equal(3, categoriesNum);
        }
        [Fact]
        public async Task DeleteCategory_NonExistentId()
        {
            var service = GetService();
            await service.DeleteCategory(99);
            var categoriesNum = _context.Categories.Count();
            Assert.Equal(4, categoriesNum);
        }
        [Fact]
        public async Task UpdateCategory()
        {
            var service = GetService();
            var categoryUpdated = new CategoryDTO()
            {
                Name = "Buckets",
                Image = "URL",
            };
            await service.UpdateCategory(4, categoryUpdated);
            var updatedCategory = await _context.Categories.FindAsync(4);
            Assert.NotNull(updatedCategory);
            Assert.Equal(updatedCategory.Image, categoryUpdated.Image);
        }
        [Fact]
        public async Task UpdateCategory_ParentCategory()
        {
            var service = GetService();
            var categoryUpdated = new CategoryDTO()
            {
                Name = "Buckets",
                Image = "",
                ParentCategoryId = 1
            };
            await service.UpdateCategory(4, categoryUpdated);
            var updatedCategory = await _context.Categories.FindAsync(4);
            Assert.NotNull(updatedCategory);
            Assert.NotNull(updatedCategory.ParentCategory);
        }
        [Fact]
        public async Task UpdateCategory_WrongId()
        {
            var service = GetService();
            var categoryUpdated = new CategoryDTO()
            {
                Name = "Buckets",
                Image = "",
            };
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateCategory(10,categoryUpdated));
        }

        private ICatalogService GetService()
        {
            var mockMQ = new Mock<IMQClient>();
            mockMQ.Setup(s => s.PublishItemUpdated(It.IsAny<MessagingService.Contracts.Item>()));
            mockMQ.Setup(s => s.PublishItemDeleted(It.IsAny<int>()));
            return new CatalogEFService(_context, _mapper, mockMQ.Object);
        }
    }
}