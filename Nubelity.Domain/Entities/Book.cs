

using System.Reflection;

namespace Nubelity.Domain.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; }  
        public string Isbn { get; set; }
        public Guid AuthorId { get; set; }
        public int PageNumber { get; set; }
        public string CoverUrl { get; set; }

        public Author Author { get; set; }


}
}
