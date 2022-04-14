using System.Collections.Generic;
using System.Threading.Tasks;
using HarvestForecast.Client.Entities;

namespace HarvestForecast.Client;

/// <summary>
///     A client which provides strongly typed access to the Harvest Forecast (unofficial) API.
/// </summary>
public interface IForecastClient
{
    /// <summary>
    ///     Gets the <see cref="CurrentUser" /> object for the logged in user.
    /// </summary>
    ValueTask<CurrentUser> WhoAmIAsync();

    /// <summary>
    ///     Gets the <see cref="GetAccountAsync" /> object for the active account.
    /// </summary>
    ValueTask<Account> GetAccountAsync();

    /// <summary>
    ///     Gets the assignments specified by the <paramref name="filter" />.
    /// </summary>
    /// <param name="filter">The options to filter by.</param>
    ValueTask<IReadOnlyCollection<Assignment>> GetAssignmentsAsync( AssignmentFilter filter );

    /// <summary>
    ///     Gets all of the projects.
    /// </summary>
    ValueTask<IReadOnlyCollection<Project>> GetProjectsAsync();

    /// <summary>
    ///     Gets a project by ID or null if one is not found with that ID.
    /// </summary>
    ValueTask<Project?> GetProjectAsync( int id );

    /// <summary>
    ///     Gets all of the clients.
    /// </summary>
    ValueTask<IReadOnlyCollection<Entities.Client>> GetClientsAsync();

    /// <summary>
    ///     Gets a client by ID or null if one is not found with that ID.
    /// </summary>
    ValueTask<Entities.Client?> GetClientAsync( int id );

    /// <summary>
    ///     Gets all of the milestones.
    /// </summary>
    ValueTask<IReadOnlyCollection<Milestone>> GetMilestonesAsync();

    /// <summary>
    ///     Gets the milestones specified by the <paramref name="filter" />.
    /// </summary>
    ValueTask<IReadOnlyCollection<Milestone>> GetMilestonesAsync( MilestoneFilter filter );

    /// <summary>
    ///     Gets all of the people.
    /// </summary>
    ValueTask<IReadOnlyCollection<Person>> GetPeopleAsync();

    /// <summary>
    ///     Gets a single person or null if one is not found with that ID.
    /// </summary>
    ValueTask<Person?> GetPersonAsync( int id );
}
