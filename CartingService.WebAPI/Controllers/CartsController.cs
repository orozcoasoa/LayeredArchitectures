using CartingService.Core.BLL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CartingService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartingService _service;

        public CartsController(ICartingService service)
        {
            _service = service;
        }

        // GET: api/<CartsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Item>>> Get(Guid id)
        {
            var items = await _service.GetCartItemsAsync(id);
            if(items.Any())
                return Ok(items);
            return NotFound(items);
        }

        // POST api/<CartsController>
        [HttpPost]
        public async Task<ActionResult> Post(Guid cartid)
        {
            await _service.InitializeCartAsync(cartid, null);
            return NoContent();
        }
        // POST api/<CartsController>/id
        [HttpPost("{id}")]
        public async Task<ActionResult> Post(Guid id, [FromBody] Item item)
        {
            await _service.AddItemAsync(id, item);
            return NoContent();
        }

        // DELETE api/<CartsController>/5/1
        [HttpDelete("{id}/{itemId}")]
        public async Task<ActionResult> Delete(Guid id, int itemId)
        {
            await _service.RemoveItemAsync(id, itemId);
            return NoContent();
        }
    }
}
