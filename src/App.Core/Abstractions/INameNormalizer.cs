namespace App.Core.Abstractions;

public interface INameNormalizer
{
    string NormalizeForSearch(string? value, bool removeDiacritics = true);
}
