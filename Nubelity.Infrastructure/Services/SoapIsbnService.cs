

using External.IsbnSoapClient;
using Nubelity.Application.Interfaces;

namespace Nubelity.Infrastructure.Services
{
    public class SoapIsbnService : IIsbnSoapService
    {
        private readonly SBNServiceSoapTypeClient _client;

        public SoapIsbnService(SBNServiceSoapTypeClient client)
        {
            _client = client;
        }

        public async Task<bool> ValidateIsbnAsync(string isbn)
        {
            try
            {
                var result = await _client.IsValidISBN13Async(isbn);
                return result.Body.IsValidISBN13Result;
            }
            catch
            {
                return false;
            }
        }
    }
}
