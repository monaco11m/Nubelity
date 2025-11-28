using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nubelity.Application.DTOs;
using Nubelity.Application.DTOs.Books;
using Nubelity.Application.Interfaces;

namespace Nubelity.API.Controllers
{
    [ApiController]
    [Route("api/books")]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<BookDto>>> GetAll(
            int page = 1,
            int pageSize = 10,
            string? title = null,
            string? author = null)
        {
            var result = await _bookService.GetAllAsync(page, pageSize, title, author);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BookDto>> GetById(Guid id)
        {
            var result = await _bookService.GetByIdAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> Create([FromBody] BookCreateDto dto)
        {
            var result = await _bookService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPatch("{id:guid}")]
        public async Task<ActionResult<BookDto>> Update(Guid id, [FromBody] BookUpdateDto dto)
        {
            var result = await _bookService.UpdatePartialAsync(id, dto);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookService.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("validation/{isbn}")]
        public async Task<ActionResult<bool>> ValidateIsbn(string isbn)
        {
            var isValid = await _bookService.ValidateIsbnAsync(isbn);
            return Ok(new { isbn, isValid });
        }

        [HttpPost("massive")]
        public async Task<ActionResult<IEnumerable<BookDto>>> CreateMassive([FromForm] BookMassiveUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("CSV file is required.");

            using var stream = dto.File.OpenReadStream();

            var result = await _bookService.CreateMassiveAsync(stream);

            return Ok(result);
        }
    }
}
