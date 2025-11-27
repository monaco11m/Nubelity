
using Microsoft.EntityFrameworkCore;
using Nubelity.Application.Interfaces;
using Nubelity.Domain.Entities;

namespace Nubelity.Infrastructure.Persistence.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;

        public AuthorRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<Author> GetByIdAsync(Guid id)
        {
            return await _context.Authors
                .Include(a => a.Books)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _context.Authors
                .Include(a => a.Books)
                .ToListAsync();
        }

        public async Task AddAsync(Author author)
        {
            await _context.Authors.AddAsync(author);
        }

        public void Update(Author author)
        {
            _context.Authors.Update(author);
        }

        public void Delete(Author author)
        {
            _context.Authors.Remove(author);
        }
    }
}
