
using Nubelity.Application.DTOs;
using Nubelity.Application.DTOs.Authors;
using Nubelity.Application.Exceptions;
using Nubelity.Application.Interfaces;
using Nubelity.Domain.Entities;

namespace Nubelity.Application.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INormalizationService _normalizer;

        public AuthorService(IUnitOfWork unitOfWork, INormalizationService normalizer)
        {
            _unitOfWork = unitOfWork;
            _normalizer = normalizer;
        }

        public async Task<PagedResult<AuthorDto>> GetAllAsync(int pageNumber, int pageSize)
        {
            var authors = await _unitOfWork.AuthorRepository.GetAllAsync();

            var totalCount = authors.Count();

            var paged = authors
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(a => new AuthorDto
                {
                    Id = a.Id,
                    FullName = a.FullName,
                    BirthDate = a.BirthDate
                });

            return new PagedResult<AuthorDto>
            {
                Items = paged,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<AuthorDto> GetByIdAsync(Guid id)
        {
            var author = await _unitOfWork.AuthorRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Author not found");

            return new AuthorDto
            {
                Id = author.Id,
                FullName = author.FullName,
                BirthDate = author.BirthDate
            };
        }


        public async Task<AuthorDto> CreateAsync(AuthorCreateDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new DomainValidationException("Author name cannot be empty");

            var normalizedName = _normalizer.NormalizeText(dto.FullName);

            var author = new Author
            {
                Id = Guid.NewGuid(),
                FullName = normalizedName,
                BirthDate = dto.BirthDate
            };

            await _unitOfWork.AuthorRepository.AddAsync(author);
            await _unitOfWork.SaveChangesAsync();

            return new AuthorDto
            {
                Id = author.Id,
                FullName = author.FullName,
                BirthDate = author.BirthDate
            };
        }

        public async Task<AuthorDto> UpdatePartialAsync(Guid id, AuthorUpdateDto dto)
        {
            var author = await _unitOfWork.AuthorRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Author not found");

            if (dto.FullName != null)
                author.FullName = _normalizer.NormalizeText(dto.FullName);

            if (dto.BirthDate.HasValue)
                author.BirthDate = dto.BirthDate.Value;

            _unitOfWork.AuthorRepository.Update(author);
            await _unitOfWork.SaveChangesAsync();

            return new AuthorDto
            {
                Id = author.Id,
                FullName = author.FullName,
                BirthDate = author.BirthDate
            };
        }


        public async Task DeleteAsync(Guid id)
        {
            var author = await _unitOfWork.AuthorRepository.GetByIdAsync(id)
                ?? throw new NotFoundException("Author not found");

            _unitOfWork.AuthorRepository.Delete(author);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
