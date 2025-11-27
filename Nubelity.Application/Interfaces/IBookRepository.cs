
using Nubelity.Domain.Entities;

namespace Nubelity.Application.Interfaces
{
    public interface IBookRepository
    {
        Task<Book> GetByIdAsync(Guid id);
        Task<IEnumerable<Book>> GetAllAsync();
        Task<IEnumerable<Book>> SearchAsync(string? title, string? author);
        Task AddAsync(Book book);
        void Update(Book book);
        void Delete(Book book);
    }
}
