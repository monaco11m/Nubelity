namespace Nubelity.Application.DTOs.Books
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Isbn { get; set; }
        public int PageNumber { get; set; }
        public string CoverUrl { get; set; }
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}
