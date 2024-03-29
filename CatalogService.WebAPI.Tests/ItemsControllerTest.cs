﻿using CatalogService.BLL;
using CatalogService.BLL.Entities;
using CatalogService.WebAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CatalogService.WebAPI.Tests
{
    public class ItemsControllerTest
    {
        [Fact]
        public void GetItems()
        {
            Assert.True(true);
        }
        [Fact]
        public async Task GetItem_Ok()
        {
            var itemToReturn = new Item() { Id = 1, Name = "Item1" };
            var serviceMock = new Mock<ICatalogService>();
            serviceMock.Setup(s => s.GetItem(itemToReturn.Id))
                .Returns(Task.FromResult(itemToReturn));

            var controller = new ItemsController(serviceMock.Object);
            var result = await controller.GetItem(itemToReturn.Id);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.NotNull(okResult.Value);
            var item = (Item)okResult.Value;
            Assert.Equal(itemToReturn.Id, item.Id);
            Assert.Equal(itemToReturn.Name, item.Name);
        }
        [Fact]
        public async Task GetItem_NotFound()
        {
            var serviceMock = new Mock<ICatalogService>();
            serviceMock.Setup(s => s.GetItem(1))
                .Returns(Task.FromResult((Item)null));

            var controller = new ItemsController(serviceMock.Object);
            var result = await controller.GetItem(1);
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
            var notFoundResult = result.Result as NotFoundResult;
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }
        [Fact]
        public async Task GetItemDetails_Ok()
        {
            var itemDetailsToReturn = new ItemDetails() { ItemId = 1, Details = new Dictionary<string, string>() };
            var serviceMock = new Mock<ICatalogService>();
            serviceMock.Setup(s => s.GetItemDetails(itemDetailsToReturn.ItemId))
                .Returns(Task.FromResult(itemDetailsToReturn));

            var controller = new ItemsController(serviceMock.Object);
            var result = await controller.GetItemDetails(itemDetailsToReturn.ItemId);
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.NotNull(okResult.Value);
            var itemDetails = (ItemDetails)okResult.Value;
            Assert.Equal(itemDetailsToReturn.ItemId, itemDetails.ItemId);
            Assert.Equal(itemDetailsToReturn.Details, itemDetails.Details);
        }
        [Fact]
        public async Task GetItemDetails_NotFound()
        {
            var serviceMock = new Mock<ICatalogService>();
            serviceMock.Setup(s => s.GetItemDetails(1))
                .Returns(Task.FromResult((ItemDetails)null));

            var controller = new ItemsController(serviceMock.Object);
            var result = await controller.GetItemDetails(1);
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result.Result);
            var notFoundResult = result.Result as NotFoundResult;
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }
        [Fact]
        public async Task AddItem()
        {
            var newItem = new ItemDTO() { Name = "Item1" };
            var serviceMock = new Mock<ICatalogService>();
            serviceMock.Setup(s => s.AddItem(newItem))
                .Returns(Task.FromResult(new Item()
                {
                    Id = 1,
                    Name = newItem.Name
                }));

            var controller = new ItemsController(serviceMock.Object);
            var result = await controller.AddItem(newItem);
            Assert.NotNull(result);
            Assert.IsType<CreatedAtActionResult>(result.Result);
            var createdResult = result.Result as CreatedAtActionResult;
            Assert.Equal(StatusCodes.Status201Created, createdResult.StatusCode);
            Assert.NotNull(createdResult.Value);
            var item = (Item)createdResult.Value;
            Assert.Equal(1, item.Id);
        }
        [Fact]
        public async Task UpdateItem()
        {
            var updatedItem = new ItemDTO() { Name = "Item1" };
            var serviceMock = new Mock<ICatalogService>();
            serviceMock.Setup(s => s.UpdateItem(1, updatedItem))
                .Returns(Task.CompletedTask);

            var controller = new ItemsController(serviceMock.Object);
            var result = await controller.UpdateItem(1, updatedItem);
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
            var noContentResult = result as NoContentResult;
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }
        [Fact]
        public async Task DeleteItem()
        {
            var serviceMock = new Mock<ICatalogService>();
            serviceMock.Setup(s => s.DeleteItem(1))
                .Returns(Task.CompletedTask);

            var controller = new ItemsController(serviceMock.Object);
            var result = await controller.DeleteItem(1);
            Assert.NotNull(result);
            Assert.IsType<NoContentResult>(result);
            var noContentResult = result as NoContentResult;
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }
    }
}
