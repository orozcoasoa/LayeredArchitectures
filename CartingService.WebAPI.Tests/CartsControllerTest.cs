using CartingService.BLL;
using CartingService.WebAPI.Controllers.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CartingService.WebAPI.Tests
{
    public class CartsControllerTest
    {
        [Fact]
        public async Task GetCart_Ok()
        {
            var guid = Guid.NewGuid();
            var serviceMock = new Mock<ICartingService>();
            serviceMock.Setup(s => s.GetCart(guid))
                .Returns(Task.FromResult(new Cart()
                {
                    Id = guid,
                    Items = new List<Item>()
                    {
                        new Item() { Id = 1, Name = "Item1"}
                    }
                }));

            var controller = new CartsController(serviceMock.Object);
            var result = await controller.GetCart(guid);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.NotNull(okResult.Value);
            var cart = (Cart)okResult.Value;
            Assert.NotNull(cart);
            Assert.Equal(guid, cart.Id);
            Assert.Single(cart.Items);
        }
        [Fact]
        public async Task GetCart_NotFound()
        {
            var guid = Guid.NewGuid();
            var serviceMock = new Mock<ICartingService>();
            serviceMock.Setup(s => s.GetCart(guid))
                .Returns(Task.FromResult((Cart)null));

            var controller = new CartsController(serviceMock.Object);
            var result = await controller.GetCart(guid);
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
            var notFoundResult = result.Result as NotFoundResult;
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }
        [Fact]
        public async Task AddItem_ExistingCart()
        {
            var guid = Guid.NewGuid();
            var itemToAdd = new Item() { Id = 1, Name = "Item1" };
            var serviceMock = new Mock<ICartingService>();
            serviceMock.Setup(s => s.ExistsCart(guid))
                .Returns(Task.FromResult(true));
            serviceMock.Setup(s => s.AddItem(guid, itemToAdd))
                .Returns(Task.CompletedTask);
            serviceMock.Setup(s => s.InitializeCart(guid, itemToAdd))
                .Returns(Task.FromResult(new Cart() { Id = guid, 
                                            Items = new List<Item>()}));

            var controller = new CartsController(serviceMock.Object);
            var result = await controller.AddItemtoCart(guid,itemToAdd);
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
            var okResult = result as OkResult;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }
        [Fact]
        public async Task AddItem_NonExistingCart()
        {
            var guid = Guid.NewGuid();
            var itemToAdd = new Item() { Id = 1, Name = "Item1" };
            var serviceMock = new Mock<ICartingService>();
            serviceMock.Setup(s => s.ExistsCart(guid))
                .Returns(Task.FromResult(false));
            serviceMock.Setup(s => s.AddItem(guid, itemToAdd))
                .Returns(Task.CompletedTask);
            serviceMock.Setup(s => s.InitializeCart(guid, itemToAdd))
                .Returns(Task.FromResult(new Cart()
                {
                    Id = guid,
                    Items = new List<Item>()
                }));

            var controller = new CartsController(serviceMock.Object);
            var result = await controller.AddItemtoCart(guid, itemToAdd);
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
            var okResult = result as OkResult;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }
        [Fact]
        public async Task RemoveItem_Ok()
        {
            var guid = Guid.NewGuid();
            var serviceMock = new Mock<ICartingService>();
            serviceMock.Setup(s => s.ExistsItemOnCart(guid,1))
                .Returns(Task.FromResult(true));
            serviceMock.Setup(s => s.RemoveItem(guid, 1))
                .Returns(Task.CompletedTask);

            var controller = new CartsController(serviceMock.Object);
            var result = await controller.RemoveItemFromCart(guid, 1);
            Assert.NotNull(result);
            Assert.IsType<OkResult>(result);
            var okResult = result as OkResult;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }
        [Fact]
        public async Task RemoveItem_NoContent()
        {
            var guid = Guid.NewGuid();
            var serviceMock = new Mock<ICartingService>();
            serviceMock.Setup(s => s.ExistsItemOnCart(guid, 1))
                .Returns(Task.FromResult(false));

            var controller = new CartsController(serviceMock.Object);
            var result = await controller.RemoveItemFromCart(guid, 1);
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
            var noContentResult = result as NoContentResult;
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }
    }
}