using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using App.Core.Abstractions;

namespace App.Core.Services;

public sealed partial class SearchNameNormalizer : INameNormalizer
{
    [GeneratedRegex("\\s+", RegexOptions.Compiled)]
    private static partial Regex WhitespaceRegex();

    public string NormalizeForSearch(string? value, bool removeDiacritics = true)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return string.Empty;
        }

        var collapsedWhitespace = WhitespaceRegex().Replace(value, " ").Trim();

        if (!removeDiacritics)
        {
            return collapsedWhitespace.ToLowerInvariant();
        }

        var normalized = collapsedWhitespace.Normalize(NormalizationForm.FormD);
        var builder = new StringBuilder(normalized.Length);

        foreach (var c in normalized)
        {
            var category = CharUnicodeInfo.GetUnicodeCategory(c);
            if (category is not UnicodeCategory.NonSpacingMark)
            {
                builder.Append(c);
            }
        }

        return builder
            .ToString()
            .Normalize(NormalizationForm.FormC)
            .ToLowerInvariant();
    }
}
