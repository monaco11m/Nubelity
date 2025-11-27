

namespace Nubelity.Domain.Entities
{
    public class Author
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();

    }
}
