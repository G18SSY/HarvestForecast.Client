using System.Collections.Generic;
using System.Threading.Tasks;
using HarvestForecast.Client.Entities;
using HarvestForecast.Client.Entities.VO;

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
    ValueTask<IReadOnlyCollection<Assignment>> GetAssignmentsAsync(AssignmentFilter filter);

    /// <summary>
    ///     Creates a new assignment.
    /// </summary>
    /// <param name="assignment">The assignment to create.</param>
    ValueTask<Assignment> CreateAssignmentAsync(AssignmentData assignment);

    /// <summary>
    ///     Update an existing assignment.
    /// </summary>
    /// <param name="id">The ID of the assignment to update.</param>
    /// <param name="assignment">The assignment data to update with.</param>
    ValueTask<Assignment> UpdateAssignmentAsync(ForecastAssignmentId id, AssignmentData assignment);

    /// <summary>
    ///     Remove an existing assignment.
    /// </summary>
    /// <param name="id">The ID of the assignment to remove.</param>
    ValueTask<bool> RemoveAssignmentAsync(ForecastAssignmentId id);

    /// <summary>
    ///     Gets all of the projects.
    /// </summary>
    ValueTask<IReadOnlyCollection<Project>> GetProjectsAsync();

    /// <summary>
    ///     Gets a project by ID or null if one is not found with that ID.
    /// </summary>
    ValueTask<Project?> GetProjectAsync(ForecastProjectId id);

    /// <summary>
    ///     Gets all of the clients.
    /// </summary>
    ValueTask<IReadOnlyCollection<Entities.Client>> GetClientsAsync();

    /// <summary>
    ///     Gets a client by ID or null if one is not found with that ID.
    /// </summary>
    ValueTask<Entities.Client?> GetClientAsync(ForecastClientId id);

    /// <summary>
    ///     Gets all of the milestones.
    /// </summary>
    ValueTask<IReadOnlyCollection<Milestone>> GetMilestonesAsync();

    /// <summary>
    ///     Gets the milestones specified by the <paramref name="filter" />.
    /// </summary>
    ValueTask<IReadOnlyCollection<Milestone>> GetMilestonesAsync(MilestoneFilter filter);

    /// <summary>
    ///     Gets all of the people.
    /// </summary>
    ValueTask<IReadOnlyCollection<Person>> GetPeopleAsync();

    /// <summary>
    ///     Gets a single person or null if one is not found with that ID.
    /// </summary>
    ValueTask<Person?> GetPersonAsync(ForecastPersonId id);

    /// <summary>
    ///     Gets all of the placeholders.
    /// </summary>
    ValueTask<IReadOnlyCollection<Placeholder>> GetPlaceholdersAsync();

    /// <summary>
    ///     Gets a single placeholder or null if one is not found with that ID.
    /// </summary>
    ValueTask<Placeholder?> GetPlaceholderAsync(ForecastPlaceholderId id);

    /// <summary>
    ///     Gets all of the roles.
    /// </summary>
    ValueTask<IReadOnlyCollection<Role>> GetRolesAsync();

    /// <summary>
    ///     Gets a single role or null if one is not found with that ID.
    /// </summary>
    ValueTask<Role?> GetRoleAsync(ForecastRoleId id);
}
