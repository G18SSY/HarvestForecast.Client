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
    ///     Gets the <see cref="AccountAsync" /> object for the active account.
    /// </summary>
    ValueTask<Account> AccountAsync();

    /// <summary>
    ///     Gets the assignments specified by the <paramref name="filter" />.
    /// </summary>
    /// <param name="filter">The options to filter by.</param>
    ValueTask<IReadOnlyCollection<Assignment>> AssignmentsAsync( AssignmentFilter filter );

    /// <summary>
    ///     Gets all of the projects.
    /// </summary>
    ValueTask<IReadOnlyCollection<Project>> ProjectsAsync();

    /// <summary>
    ///     Gets a project by ID.
    /// </summary>
    ValueTask<Project> ProjectAsync( int id );
    
    /// <summary>
    ///     Gets all of the clients.
    /// </summary>
    ValueTask<IReadOnlyCollection<Entities.Client>> ClientsAsync();
    
    /// <summary>
    ///     Gets a client by ID.
    /// </summary>
    ValueTask<Entities.Client> ClientAsync(int id);
}
