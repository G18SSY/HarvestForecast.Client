using System;
using System.Globalization;
using System.Web;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     Filter options for querying <see cref="Assignment" />s.
/// </summary>
public record AssignmentFilter( DateTime StartDate, DateTime EndDate )
{
    /// <summary>
    ///     The date to filter assignments from (inclusive).
    /// </summary>
    public DateTime StartDate { get; } = StartDate;

    /// <summary>
    ///     The date to filter assignments to (inclusive).
    /// </summary>
    public DateTime EndDate { get; } = EndDate;

    /// <summary>
    ///     The ID of the project to filter assignments by.
    /// </summary>
    public int? ProjectId { get; init; }

    /// <summary>
    ///     The ID of the person to filter assignments by.
    /// </summary>
    public int? PersonId { get; init; }

    /// <summary>
    ///     The ID of the repeated assignment to filter assignments by.
    /// </summary>
    public int? RepeatedAssignmentSetId { get; init; }

    /// <summary>
    ///     The state to filter assignments by.
    /// </summary>
    public AssigmentState? State { get; init; }

    /// <summary>
    ///     Returns a basic <see cref="AssignmentFilter" /> setup to filter by today's date.
    /// </summary>
    public static AssignmentFilter Today()
    {
        return new AssignmentFilter( DateTime.Today, DateTime.Today );
    }

    /// <summary>
    ///     Builds a query string from the filters which have values.
    /// </summary>
    internal string ToQueryString()
    {
        var collection = HttpUtility.ParseQueryString( string.Empty );

        AddIfSet( "project_id", ProjectId?.ToString( CultureInfo.InvariantCulture ) );
        AddIfSet( "person_id", PersonId?.ToString( CultureInfo.InvariantCulture ) );
        AddIfSet( "start_date", DateUtility.FormatDateOnly( StartDate ) );
        AddIfSet( "end_date", DateUtility.FormatDateOnly( EndDate ) );
        AddIfSet( "repeated_assignment_set_id", RepeatedAssignmentSetId?.ToString( CultureInfo.InvariantCulture ) );
        AddIfSet( "state", State?.ToString().ToLower() );

        return collection.ToString();

        void AddIfSet( string key, string? value )
        {
            if ( value is null )
            {
                return;
            }

            collection.Add( key, value );
        }
    }
}
