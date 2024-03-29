﻿using CatalogService.BLL;
using CatalogService.BLL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly ICatalogService _service;

        public ItemsController(ICatalogService service)
        {
            _service = service;
        }

        [HttpGet(Name = nameof(GetItems))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Item>>> GetItems([FromQuery] ItemQuery itemQuery)
        {
            var items = await _service.GetItems(itemQuery);
            Response.Headers.Add("X-Pagination", items.ToPaginationHeader());
            return Ok(items);
        }

        [HttpGet("{id}", Name = nameof(GetItem))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Item>> GetItem([FromRoute] int id)
        {
            var item = await _service.GetItem(id);
            if (item == null)
                return NotFound();
            return Ok(item);
        }

        [HttpGet("{id}/details", Name = nameof(GetItemDetails))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ItemDetails>> GetItemDetails([FromRoute] int id)
        {
            var itemDetails = await _service.GetItemDetails(id);
            if (itemDetails == null)
                return NotFound();
            return Ok(itemDetails);
        }

        [Authorize(Roles = "Catalog.Manager")]
        [HttpPost(Name = nameof(AddItem))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Item>> AddItem([FromBody] ItemDTO item)
        {
            var createdItem = await _service.AddItem(item);
            return CreatedAtAction(nameof(AddItem), new { id = createdItem.Id }, createdItem);
        }

        [Authorize(Roles = "Catalog.Manager")]
        [HttpPut("{id}", Name = nameof(UpdateItem))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateItem([FromRoute] int id, [FromBody] ItemDTO item)
        {
            await _service.UpdateItem(id, item);
            return NoContent();
        }

        [Authorize(Roles = "Catalog.Manager")]
        [HttpDelete("{id}", Name = nameof(DeleteItem))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteItem([FromRoute] int id)
        {
            await _service.DeleteItem(id);
            return NoContent();
        }
    }
}
