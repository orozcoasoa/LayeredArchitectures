using CatalogService.BLL;
using CatalogService.BLL.Entities;
using CatalogService.WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CatalogService.WebAPI.Tests
{
    public class CategoriesControllerTest
    {
        [Fact]
        public async Task GetCategoriesOk()
        {
            var serviceMock = new Mock<ICatalogService>();
            serviceMock.Setup(s => s.GetAllCategories())
                .Returns(Task.FromResult(new List<Category>()
                {
                    new Category() {Id =1, Name="Category1"},
                    new Category() {Id =2, Name="Category2", Image=""},
                    new Category() {Id =3, Name="Category3", Image="",
                        ParentCategory=new Category() {Id = 1 } },
                }));

            var controller = new CategoriesController(serviceMock.Object);
            var result = await controller.GetCategories();
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.NotNull(okResult.Value);
            var categories = (IEnumerable<Category>)okResult.Value;
            Assert.Equal(3, categories.Count());
        }
        [Fact]
        public async Task GetCategoriesOk_Empty()
        {
            var serviceMock = new Mock<ICatalogService>();
            serviceMock.Setup(s => s.GetAllCategories())
                .Returns(Task.FromResult(new List<Category>()));

            var controller = new CategoriesController(serviceMock.Object);
            var result = await controller.GetCategories();
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.NotNull(okResult.Value);
            var categories = (IEnumerable<Category>)okResult.Value;
            Assert.Empty(categories);
        }
        [Fact]
        public async Task AddCategory()
        {
            var newCategory = new CategoryDTO() { Name = "Category1" };
            var serviceMock = new Mock<ICatalogService>();
            serviceMock.Setup(s => s.AddCategory(newCategory))
                .Returns(Task.FromResult(new Category()
                {
                    Id = 1,
                    Name = newCategory.Name
                }));

            var controller = new CategoriesController(serviceMock.Object);
            var result = await controller.AddCategory(newCategory);
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.NotNull(createdResult.Value);
            var category = (Category)createdResult.Value;
            Assert.Equal(1, category.Id);
        }
        [Fact]
        public async Task UpdateCategory()
        {
            var updatedCategory = new CategoryDTO() { Name = "Category1" };
            var serviceMock = new Mock<ICatalogService>();
            serviceMock.Setup(s => s.UpdateCategory(1, updatedCategory))
                .Returns(Task.CompletedTask);

            var controller = new CategoriesController(serviceMock.Object);
            var result = await controller.UpdateCategory(1, updatedCategory);
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
            var noContentResult = result as NoContentResult;
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }
        [Fact]
        public async Task DeleteCategory()
        {
            var serviceMock = new Mock<ICatalogService>();
            serviceMock.Setup(s => s.DeleteCategory(1))
                .Returns(Task.CompletedTask);

            var controller = new CategoriesController(serviceMock.Object);
            var result = await controller.DeleteCategory(1);
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
            var noContentResult = result as NoContentResult;
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }
    }
}