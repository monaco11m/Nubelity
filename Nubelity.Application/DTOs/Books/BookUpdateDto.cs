namespace Nubelity.Application.DTOs.Books
{
    public class BookUpdateDto
    {
        public string? Title { get; set; }
        public string? Isbn { get; set; }
        public int? PageNumber { get; set; }
        public Guid? AuthorId { get; set; }
    }
}
