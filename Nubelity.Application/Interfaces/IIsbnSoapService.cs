
namespace Nubelity.Application.Interfaces
{
    public interface IIsbnSoapService
    {
        Task<bool> ValidateIsbnAsync(string isbn);
    }
}
