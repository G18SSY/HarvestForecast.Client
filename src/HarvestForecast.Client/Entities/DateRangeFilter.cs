using System;
using System.Collections.Generic;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     A <see cref="FilterBase" /> implementation for filters that require a start and end date.
/// </summary>
public record DateRangeFilter( DateOnly StartDate, DateOnly EndDate ) : FilterBase
{
    /// <summary>
    ///     The date to filter assignments from (inclusive).
    /// </summary>
    public DateOnly StartDate { get; } = StartDate;

    /// <summary>
    ///     The date to filter assignments to (inclusive).
    /// </summary>
    public DateOnly EndDate { get; } = EndDate;

    internal override IEnumerable<KeyValuePair<string, string?>> GetFilters()
    {
        yield return new KeyValuePair<string, string?>( "start_date", DateUtility.FormatDateOnly( StartDate ) );
        yield return new KeyValuePair<string, string?>( "end_date", DateUtility.FormatDateOnly( EndDate ) );
    }
}
