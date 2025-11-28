using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nubelity.Application.DTOs.Authors;
using Nubelity.Application.Interfaces;

namespace Nubelity.API.Controllers
{
    [ApiController]
    [Route("api/authors")]
    [Authorize]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAll(
            int page = 1,
            int pageSize = 10)
        {
            var result = await _authorService.GetAllAsync(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<AuthorDto>> GetById(Guid id)
        {
            var result = await _authorService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<AuthorDto>> Create([FromBody] AuthorCreateDto dto)
        {
            var result = await _authorService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<AuthorDto>> Update(Guid id, [FromBody] AuthorUpdateDto dto)
        {
            var result = await _authorService.UpdatePartialAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _authorService.DeleteAsync(id);
            return NoContent();
        }
    }
}
