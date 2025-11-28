

using Nubelity.Application.DTOs;
using Nubelity.Application.DTOs.Authors;

namespace Nubelity.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<PagedResult<AuthorDto>> GetAllAsync(int pageNumber, int pageSize);
        Task<AuthorDto> GetByIdAsync(Guid id);
        Task<AuthorDto> CreateAsync(AuthorCreateDto dto);
        Task<AuthorDto> UpdatePartialAsync(Guid id, AuthorUpdateDto dto);
        Task DeleteAsync(Guid id);
    }
}
