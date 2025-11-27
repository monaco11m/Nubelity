

using Nubelity.Application.Interfaces;

namespace Nubelity.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly LibraryDbContext _context;

        public IAuthorRepository AuthorRepository { get; }
        public IBookRepository BookRepository { get; }
        public IUserRepository UserRepository { get; }

        public UnitOfWork(
            LibraryDbContext context,
            IAuthorRepository authorRepository,
            IBookRepository bookRepository,
            IUserRepository userRepository)
        {
            _context = context;

            AuthorRepository = authorRepository;
            BookRepository = bookRepository;
            UserRepository = userRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
