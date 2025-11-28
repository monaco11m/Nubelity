using Nubelity.Application.Interfaces;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Nubelity.Infrastructure.Services
{
    public class NormalizationService : INormalizationService
    {
        public string NormalizeText(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            string text = input.Trim();

            text = Regex.Replace(text, @"\s+", " ");

            text = Regex.Replace(text, @"[0-9]", "");

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark);
            text = new string(chars.ToArray());

            text = text.ToUpperInvariant();

            return text;
        }
    }
}
