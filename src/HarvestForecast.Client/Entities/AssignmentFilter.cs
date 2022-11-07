using System;
using System.Collections.Generic;
using System.Globalization;
using HarvestForecast.Client.Entities.VO;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     Filter options for querying <see cref="Assignment" />s.
/// </summary>
public record AssignmentFilter(DateOnly StartDate, DateOnly EndDate) : DateRangeFilter(StartDate, EndDate)
{
    /// <summary>
    ///     The ID of the project to filter assignments by.
    /// </summary>
    public ForecastProjectId? ProjectId { get; init; }

    /// <summary>
    ///     The ID of the person to filter assignments by.
    /// </summary>
    public ForecastPersonId? PersonId { get; init; }

    /// <summary>
    ///     The ID of the repeated assignment to filter assignments by.
    /// </summary>
    public ForecastRepeatedAssignmentSetId? RepeatedAssignmentSetId { get; init; }

    /// <summary>
    ///     The state to filter assignments by.
    /// </summary>
    public AssigmentState? State { get; init; }

    /// <summary>
    ///     Returns a basic <see cref="AssignmentFilter" /> setup to filter by today's date.
    /// </summary>
    public static AssignmentFilter Today()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return new AssignmentFilter(today, today);
    }

    internal override IEnumerable<KeyValuePair<string, string?>> GetFilters()
    {
        foreach (var basePair in base.GetFilters()) yield return basePair;

        yield return new KeyValuePair<string, string?>("project_id",
            ProjectId?.Value.ToString(CultureInfo.InvariantCulture));
        yield return new KeyValuePair<string, string?>("person_id",
            PersonId?.Value.ToString(CultureInfo.InvariantCulture));
        yield return new KeyValuePair<string, string?>("repeated_assignment_set_id",
            RepeatedAssignmentSetId?.Value.ToString(CultureInfo.InvariantCulture));
        yield return new KeyValuePair<string, string?>("state", State?.ToString().ToLower());
    }
}
