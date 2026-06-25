using App.Core.Enums;
using App.Core.Models;

namespace App.Core.Abstractions;

public interface IConnectionStringBuilderService
{
    string Build(DatabaseProvider provider, DatabaseConnectionSettings settings);

    bool TryParse(DatabaseProvider provider, string connectionString, out DatabaseConnectionSettings settings);
}
