using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Json;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     Indicates the days of the week that a <see cref="Person" /> works.
/// </summary>
[ JsonConverter( typeof( WorkingDaysConverter ) ) ]
public readonly struct WorkingDays
{
    [ Flags ]
    private enum FlaggedDays
    {
        None = 0,
        Sunday = 1 << 0,
        Monday = 1 << 1,
        Tuesday = 1 << 2,
        Wednesday = 1 << 3,
        Thursday = 1 << 4,
        Friday = 1 << 5,
        Saturday = 1 << 6
    }

    private readonly FlaggedDays flags;

    private WorkingDays( FlaggedDays flags )
    {
        this.flags = flags;
    }

    /// <summary>
    ///     Creates a new <see cref="WorkingDays" /> from a collection of <see cref="DayOfWeek" />s.
    /// </summary>
    public WorkingDays( IEnumerable<DayOfWeek> days ) : this( ConvertToFlags( days ) ) { }

    private static FlaggedDays ConvertToFlags( IEnumerable<DayOfWeek> days )
    {
        var flags = FlaggedDays.None;

        foreach ( var day in days )
        {
            var flag = MapToFlag( day );
            flags |= flag;
        }

        return flags;
    }

    private static FlaggedDays MapToFlag( DayOfWeek day )
    {
        return day switch
        {
            DayOfWeek.Friday => FlaggedDays.Friday,
            DayOfWeek.Monday => FlaggedDays.Monday,
            DayOfWeek.Saturday => FlaggedDays.Saturday,
            DayOfWeek.Sunday => FlaggedDays.Sunday,
            DayOfWeek.Thursday => FlaggedDays.Thursday,
            DayOfWeek.Tuesday => FlaggedDays.Tuesday,
            DayOfWeek.Wednesday => FlaggedDays.Wednesday,
            _ => throw new ArgumentOutOfRangeException( nameof( day ), day, null )
        };
    }

    /// <summary>
    /// Checks if a specific <paramref name="day"/> is active.
    /// </summary>
    public bool IsActiveOn( DayOfWeek day )
    {
        var flag = MapToFlag(day);
        return flags.HasFlag( flag );
    }

    /// <summary>
    ///     Checks if Monday is active.
    /// </summary>
    public bool Monday => flags.HasFlag( FlaggedDays.Monday );

    /// <summary>
    ///     Checks if Tuesday is active.
    /// </summary>
    public bool Tuesday => flags.HasFlag( FlaggedDays.Tuesday );

    /// <summary>
    ///     Checks if Wednesday is active.
    /// </summary>
    public bool Wednesday => flags.HasFlag( FlaggedDays.Wednesday );

    /// <summary>
    ///     Checks if Thursday is active.
    /// </summary>
    public bool Thursday => flags.HasFlag( FlaggedDays.Thursday );

    /// <summary>
    ///     Checks if Friday is active.
    /// </summary>
    public bool Friday => flags.HasFlag( FlaggedDays.Friday );

    /// <summary>
    ///     Checks if Saturday is active.
    /// </summary>
    public bool Saturday => flags.HasFlag( FlaggedDays.Saturday );

    /// <summary>
    ///     Checks if Sunday is active.
    /// </summary>
    public bool Sunday => flags.HasFlag( FlaggedDays.Sunday );

    /// <summary>
    ///     Enumerates the days that are active.
    /// </summary>
    public IEnumerable<DayOfWeek> GetDays()
    {
        if ( Monday ) yield return DayOfWeek.Monday;
        if ( Tuesday ) yield return DayOfWeek.Tuesday;
        if ( Wednesday ) yield return DayOfWeek.Wednesday;
        if ( Thursday ) yield return DayOfWeek.Thursday;
        if ( Friday ) yield return DayOfWeek.Friday;
        if ( Saturday ) yield return DayOfWeek.Saturday;
        if ( Sunday ) yield return DayOfWeek.Sunday;
    }

    /// <summary>
    ///     Makes a <paramref name="day" /> active and returns a new <see cref="WorkingDays" />.
    /// </summary>
    public static WorkingDays operator +( WorkingDays days, DayOfWeek day )
    {
        var flag = MapToFlag( day );
        var flags = days.flags | flag;

        return new WorkingDays( flags );
    }

    /// <summary>
    ///     Makes a <paramref name="day" /> inactive and returns a new <see cref="WorkingDays" />.
    /// </summary>
    public static WorkingDays operator -( WorkingDays days, DayOfWeek day )
    {
        var flag = MapToFlag( day );
        var flags = days.flags & ~ flag;

        return new WorkingDays( flags );
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return string.Join( " | ", GetDays().Select( d => d.ToString() ) );
    }

    /// <inheritdoc />
    public override bool Equals( object? obj )
    {
        return obj is WorkingDays other &&
               flags == other.flags;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return (int) flags;
    }
}
