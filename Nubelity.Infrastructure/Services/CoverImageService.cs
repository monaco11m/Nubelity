

using Nubelity.Application.Interfaces;
using System.Text.Json;

namespace Nubelity.Infrastructure.Services
{
    public class CoverImageService : ICoverImageService
    {
        private readonly HttpClient _http;

        public CoverImageService(HttpClient http)
        {
            _http = http;
        }

        public async Task<string?> GetCoverUrlAsync(string isbn)
        {
            var url = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";

            var json = await _http.GetStringAsync(url);

            if (string.IsNullOrWhiteSpace(json))
                return null;

            using var doc = JsonDocument.Parse(json);

            string key = $"ISBN:{isbn}";

            if (!doc.RootElement.TryGetProperty(key, out var bookInfo))
                return null;

            if (bookInfo.TryGetProperty("cover", out var cover) &&
                cover.TryGetProperty("medium", out var mediumUrl))
            {
                return mediumUrl.GetString();
            }

            return null;
        }
    }
}
