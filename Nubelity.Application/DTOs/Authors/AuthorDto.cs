
namespace Nubelity.Application.DTOs.Authors
{
    public class AuthorDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public int BooksCount { get; set; }
    }
}
