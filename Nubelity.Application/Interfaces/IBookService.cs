
using Nubelity.Application.DTOs;
using Nubelity.Application.DTOs.Books;

namespace Nubelity.Application.Interfaces
{
    public interface IBookService
    {
        Task<PagedResult<BookDto>> GetAllAsync(int pageNumber, int pageSize, string? title, string? author);
        Task<BookDto> GetByIdAsync(Guid id);
        Task<BookDto> CreateAsync(BookCreateDto dto);
        Task<BookDto> UpdatePartialAsync(Guid id, BookUpdateDto dto);
        Task DeleteAsync(Guid id);

        Task<bool> ValidateIsbnAsync(string isbn);

        Task<IEnumerable<BookDto>> CreateMassiveAsync(Stream csvStream);
    }
}
