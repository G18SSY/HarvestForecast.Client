using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     Abstract base for classes which contain filtering options that should be convertible to a query string.
/// </summary>
public abstract record FilterBase
{
    internal string GetFilterQuery()
    {
        var collection = HttpUtility.ParseQueryString( string.Empty );

        foreach ( var pair in GetFilters().Where( p => p.Value is not null ) )
        {
            collection.Add( pair.Key, pair.Value );
        }

        return collection.ToString();
    }

    /// <summary>
    ///     Gets the values of the filters in an implemented class.
    /// </summary>
    internal abstract IEnumerable<KeyValuePair<string, string?>> GetFilters();
}
