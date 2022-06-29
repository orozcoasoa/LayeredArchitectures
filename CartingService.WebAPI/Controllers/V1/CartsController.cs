using CartingService.BLL;
using Microsoft.AspNetCore.Mvc;

namespace CartingService.WebAPI.Controllers.V1
{
    /// <summary>
    /// Controller for cart.
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartingService _service;

        public CartsController(ICartingService service)
        {
            _service = service;
        }

        /// <summary>
        /// Get cart with list of items.
        /// </summary>
        /// <param name="cartId">Unique id of cart.</param>
        /// <returns>Cart with list of items.</returns>
        /// <response code="200">Cart retrieved successfully</response>
        /// <response code="404">Cart not found</response>
        /// <response code="500">A server fault occurred</response>
        /// <remarks>
        /// Sample request:
        /// GET: api/v1/CartsController/5
        /// </remarks>
        [HttpGet("{cartId}", Name = nameof(GetCart))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Cart>> GetCart([FromRoute] Guid cartId)
        {
            var cart = await _service.GetCart(cartId);
            if (cart == null)
                return NotFound();
            return Ok(cart);
        }

        /// <summary>
        /// Adds an item to the cart, if cart doesn't exist, 
        /// it is initialized.
        /// </summary>
        /// <param name="cartId">Unique cart id</param>
        /// <param name="item">Item to add to cart</param>
        /// <response code="200">Item added to cart successfully</response>
        /// <response code="400">Item parameters are wrong</response>
        /// <response code="404">Item not found</response>
        /// <response code="500">A server fault occurred</response>
        /// <remarks>
        /// Sample request:
        /// POST: api/v1/CartsController/5
        /// </remarks>
        [HttpPost("{cartId}", Name = nameof(AddItemtoCart))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddItemtoCart(Guid cartId, [FromBody] Item item)
        {
            var existsCart = await _service.ExistsCart(cartId);
            if (existsCart)
                await _service.AddItem(cartId, item);
            else
                await _service.InitializeCart(cartId, item);
            return Ok();
        }

        /// <summary>
        /// Removes an item from cart.
        /// </summary>
        /// <param name="cartId">Unique cart id</param>
        /// <param name="itemId">Item id</param>
        /// <response code="200">Item removed from cart successfully</response>
        /// <response code="204">Cart or item didn't exist</response>
        /// <response code="500">A server fault occurred</response>
        /// <remarks>
        /// Sample request:
        /// DELETE: api/v1/CartsController/5/1
        /// </remarks>
        [HttpDelete("{cartId}/{itemId}", Name = nameof(RemoveItemFromCart))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> RemoveItemFromCart(Guid cartId, int itemId)
        {
            var existsCart = await _service.ExistsItemOnCart(cartId, itemId);
            if (existsCart)
            {
                await _service.RemoveItem(cartId, itemId);
                return Ok();
            }
            else
            {
                return NoContent();
            }
        }
    }
}
