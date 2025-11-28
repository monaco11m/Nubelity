
using Nubelity.Application.DTOs;
using Nubelity.Application.DTOs.Books;
using Nubelity.Application.Exceptions;
using Nubelity.Application.Interfaces;
using Nubelity.Domain.Entities;

namespace Nubelity.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INormalizationService _normalizer;
        private readonly IIsbnSoapService _isbnValidator;
        private readonly ICoverImageService _coverImageService;

        public BookService(
            IUnitOfWork unitOfWork,
            INormalizationService normalizer,
            IIsbnSoapService isbnValidator,
            ICoverImageService coverImageService)
        {
            _unitOfWork = unitOfWork;
            _normalizer = normalizer;
            _isbnValidator = isbnValidator;
            _coverImageService = coverImageService;
        }

        public async Task<PagedResult<BookDto>> GetAllAsync(int pageNumber, int pageSize, string? title, string? author)
        {
            var books = await _unitOfWork.BookRepository.SearchAsync(title, author);

            var totalCount = books.Count();

            var paged = books
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BookDto
                {
                    Id = b.Id,
                    Title = b.Title,
                    Isbn = b.Isbn,
                    PageNumber = b.PageNumber,
                    CoverUrl = b.CoverUrl,
                    AuthorId = b.AuthorId,
                    AuthorName = b.Author.FullName
                });

            return new PagedResult<BookDto>
            {
                Items = paged,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<BookDto> GetByIdAsync(Guid id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Book not found");

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Isbn = book.Isbn,
                PageNumber = book.PageNumber,
                CoverUrl = book.CoverUrl,
                AuthorId = book.AuthorId,
                AuthorName = book.Author.FullName
            };
        }


        public async Task<BookDto> CreateAsync(BookCreateDto dto)
        {

            var normalizedTitle = _normalizer.NormalizeText(dto.Title);


            var isValidIsbn = await _isbnValidator.ValidateIsbnAsync(dto.Isbn);
            if (!isValidIsbn)
                throw new DomainValidationException("Invalid ISBN");

            var cover = await _coverImageService.GetCoverUrlAsync(dto.Isbn);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = normalizedTitle,
                Isbn = dto.Isbn,
                PageNumber = dto.PageNumber,
                AuthorId = dto.AuthorId,
                CoverUrl = cover ?? ""
            };

            await _unitOfWork.BookRepository.AddAsync(book);

            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(book.Id);
        }

        public async Task<BookDto> UpdatePartialAsync(Guid id, BookUpdateDto dto)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Book not found");

            if (dto.Title != null)
                book.Title = _normalizer.NormalizeText(dto.Title);

            if (dto.Isbn != null)
            {
                var valid = await _isbnValidator.ValidateIsbnAsync(dto.Isbn);
                if (!valid) throw new DomainValidationException("Invalid ISBN");

                book.Isbn = dto.Isbn;
                book.CoverUrl = await _coverImageService.GetCoverUrlAsync(dto.Isbn) ?? book.CoverUrl;
            }

            if (dto.PageNumber.HasValue)
                book.PageNumber = dto.PageNumber.Value;

            if (dto.AuthorId.HasValue)
                book.AuthorId = dto.AuthorId.Value;

            _unitOfWork.BookRepository.Update(book);
            await _unitOfWork.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            var book = await _unitOfWork.BookRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Book not found");

            _unitOfWork.BookRepository.Delete(book);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ValidateIsbnAsync(string isbn)
        {
            return await _isbnValidator.ValidateIsbnAsync(isbn);
        }

        public async Task<IEnumerable<BookDto>> CreateMassiveAsync(Stream csvStream)
        {
            var reader = new StreamReader(csvStream);
            var list = new List<BookDto>();

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();
                if (string.IsNullOrWhiteSpace(line)) continue;

                var parts = line.Split(',');

                if (parts.Length < 4)
                    throw new DomainValidationException("CSV format must be: Title,ISBN,PageNumber,AuthorName");

                var title = _normalizer.NormalizeText(parts[0]);
                var isbn = parts[1];
                var pageNumber = int.Parse(parts[2]);
                var authorName = _normalizer.NormalizeText(parts[3]);

                if (!await _isbnValidator.ValidateIsbnAsync(isbn))
                    continue;

                var authors = await _unitOfWork.AuthorRepository.GetAllAsync();
                var author = authors.FirstOrDefault(a => a.FullName == authorName);

                if (author == null)
                {
                    author = new Author
                    {
                        Id = Guid.NewGuid(),
                        FullName = authorName,
                        BirthDate = DateTime.UtcNow 
                    };

                    await _unitOfWork.AuthorRepository.AddAsync(author);
                }

                var cover = await _coverImageService.GetCoverUrlAsync(isbn);

                var book = new Book
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Isbn = isbn,
                    PageNumber = pageNumber,
                    AuthorId = author.Id,
                    CoverUrl = cover ?? ""
                };

                await _unitOfWork.BookRepository.AddAsync(book);

                list.Add(new BookDto
                {
                    Id = book.Id,
                    Title = title,
                    Isbn = isbn,
                    PageNumber = pageNumber,
                    CoverUrl = cover ?? "",
                    AuthorId = author.Id,
                    AuthorName = authorName
                });
            }

            await _unitOfWork.SaveChangesAsync();

            return list;
        }
    }
}
