using Microsoft.AspNetCore.Http;


namespace Nubelity.Application.DTOs.Books
{
    public class BookMassiveUploadDto
    {
        public IFormFile File { get; set; }
    }
}
