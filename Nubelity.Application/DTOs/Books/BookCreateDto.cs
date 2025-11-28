namespace Nubelity.Application.DTOs.Books
{
    public class BookCreateDto
    {
        public string Title { get; set; }
        public string Isbn { get; set; }
        public int PageNumber { get; set; }
        public Guid AuthorId { get; set; }
    }
}
