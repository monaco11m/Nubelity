using FluentAssertions;
using Moq;
using Nubelity.Application.DTOs.Books;
using Nubelity.Application.Exceptions;
using Nubelity.Application.Interfaces;
using Nubelity.Application.Services;
using Xunit;

namespace Nubelity.Tests.Services;

public class BookServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWork = new();
    private readonly Mock<INormalizationService> _normalizer = new();
    private readonly Mock<IIsbnSoapService> _isbnValidator = new();
    private readonly Mock<ICoverImageService> _cover = new();

    [Fact]
    public async Task CreateAsync_ShouldThrow_WhenIsbnIsInvalid()
    {
        var service = new BookService(
            _unitOfWork.Object,
            _normalizer.Object,
            _isbnValidator.Object,
            _cover.Object
        );

        _isbnValidator
            .Setup(x => x.ValidateIsbnAsync(It.IsAny<string>()))
            .ReturnsAsync(false);

        var dto = new BookCreateDto
        {
            Title = "Test Title",
            Isbn = "123456",
            PageNumber = 100,
            AuthorId = Guid.NewGuid()
        };

        Func<Task> act = async () => await service.CreateAsync(dto);

        var ex = await act.Should()
            .ThrowAsync<DomainValidationException>();

        ex.And.Message.Should().Be("Validation failed");
        ex.And.Errors.Should().Contain("Invalid ISBN");
    }
}
