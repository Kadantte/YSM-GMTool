using App.Core.Enums;

namespace App.Core.Abstractions;

public interface IQueryStore
{
    string GetQuery(DatabaseProvider provider, QueryEntity entity);
}
