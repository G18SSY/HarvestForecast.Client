namespace HarvestForecast.Client.Entities;

/// <summary>
///     Possible states for an <see cref="Assignment" />.
/// </summary>
public enum AssigmentState
{
    /// <summary>
    ///     Active assignments.
    /// </summary>
    Active,

    /// <summary>
    ///     Previous assignments.
    /// </summary>
    Archived
}
