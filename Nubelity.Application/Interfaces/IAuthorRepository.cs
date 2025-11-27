
using Nubelity.Domain.Entities;

namespace Nubelity.Application.Interfaces
{
    public interface IAuthorRepository
    {
        Task<Author> GetByIdAsync(Guid id);
        Task<IEnumerable<Author>> GetAllAsync();
        Task AddAsync(Author author);
        void Update(Author author);
        void Delete(Author author);
    }
}
