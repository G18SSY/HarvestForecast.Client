using System.Collections.Generic;
using System.Globalization;
using HarvestForecast.Client.Entities.VO;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     Filter options for querying <see cref="Milestone" />s.
/// </summary>
public record MilestoneFilter : FilterBase
{
    /// <summary>
    ///     A filter that has no options set.
    /// </summary>
    public static MilestoneFilter None { get; } = new();

    /// <summary>
    ///     The ID of the project to filter assignments by.
    /// </summary>
    public ForecastProjectId? ProjectId { get; init; }

    internal override IEnumerable<KeyValuePair<string, string?>> GetFilters()
    {
        yield return new KeyValuePair<string, string?>("project_id",
            ProjectId?.Value.ToString(CultureInfo.InvariantCulture));
    }
}
