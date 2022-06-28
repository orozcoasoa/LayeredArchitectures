using CatalogService.BLL;
using CatalogService.BLL.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICatalogService _service;

        public CategoriesController(ICatalogService service)
        {
            _service = service;
        }

        [HttpGet(Name = nameof(GetCategories))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            var categories = await _service.GetAllCategories();
            return Ok(categories);
        }

        [HttpPost(Name = nameof(AddCategory))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Category>> AddCategory([FromBody] CategoryDTO category)
        {
            var createdCategory = await _service.AddCategory(category);
            return CreatedAtAction(nameof(AddCategory), new { id = createdCategory.Id }, createdCategory);
        }
        [HttpPut("{id}", Name = nameof(UpdateCategory))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateCategory([FromRoute] int id, [FromBody] CategoryDTO categoryDTO)
        {
            await _service.UpdateCategory(id, categoryDTO);
            return NoContent();
        }
        [HttpDelete("{id}", Name = nameof(DeleteCategory))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteCategory([FromRoute] int id)
        {
            await _service.DeleteCategory(id);
            return NoContent();
        }

    }
}
