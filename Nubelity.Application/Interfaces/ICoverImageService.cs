
namespace Nubelity.Application.Interfaces
{
    public interface ICoverImageService
    {
        Task<string?> GetCoverUrlAsync(string isbn);
    }
}
