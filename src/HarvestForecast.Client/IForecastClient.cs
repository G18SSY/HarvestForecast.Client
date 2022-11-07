using System.Collections.Generic;
using System.Threading;
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
    ValueTask<CurrentUser> WhoAmIAsync(CancellationToken ct = default);

    /// <summary>
    ///     Gets the <see cref="GetAccountAsync" /> object for the active account.
    /// </summary>
    ValueTask<Account> GetAccountAsync(CancellationToken ct = default);

    /// <summary>
    ///     Gets the assignments specified by the <paramref name="filter" />.
    /// </summary>
    /// <param name="filter">The options to filter by.</param>
    /// <param name="ct" />
    ValueTask<IReadOnlyCollection<Assignment>> GetAssignmentsAsync(AssignmentFilter filter, CancellationToken ct = default);

    /// <summary>
    ///     Creates a new assignment.
    /// </summary>
    /// <param name="assignment">The assignment to create.</param>
    /// <param name="ct" />
    ValueTask<Assignment> CreateAssignmentAsync(AssignmentData assignment, CancellationToken ct = default);

    /// <summary>
    ///     Update an existing assignment.
    /// </summary>
    /// <param name="id">The ID of the assignment to update.</param>
    /// <param name="assignment">The assignment data to update with.</param>
    /// <param name="ct" />
    ValueTask<Assignment> UpdateAssignmentAsync(ForecastAssignmentId id, AssignmentData assignment, CancellationToken ct = default);

    /// <summary>
    ///     Remove an existing assignment.
    /// </summary>
    /// <param name="id">The ID of the assignment to remove.</param>
    /// <param name="ct" />
    ValueTask<bool> RemoveAssignmentAsync(ForecastAssignmentId id, CancellationToken ct = default);

    /// <summary>
    ///     Gets all of the projects.
    /// </summary>
    ValueTask<IReadOnlyCollection<Project>> GetProjectsAsync(CancellationToken ct = default);

    /// <summary>
    ///     Gets a project by ID or null if one is not found with that ID.
    /// </summary>
    ValueTask<Project?> GetProjectAsync(ForecastProjectId id, CancellationToken ct = default);

    /// <summary>
    ///     Gets all of the clients.
    /// </summary>
    ValueTask<IReadOnlyCollection<Entities.Client>> GetClientsAsync(CancellationToken ct = default);

    /// <summary>
    ///     Gets a client by ID or null if one is not found with that ID.
    /// </summary>
    ValueTask<Entities.Client?> GetClientAsync(ForecastClientId id, CancellationToken ct = default);

    /// <summary>
    ///     Gets all of the milestones.
    /// </summary>
    ValueTask<IReadOnlyCollection<Milestone>> GetMilestonesAsync(CancellationToken ct = default);

    /// <summary>
    ///     Gets the milestones specified by the <paramref name="filter" />.
    /// </summary>
    ValueTask<IReadOnlyCollection<Milestone>> GetMilestonesAsync(MilestoneFilter filter, CancellationToken ct = default);

    /// <summary>
    ///     Gets all of the people.
    /// </summary>
    ValueTask<IReadOnlyCollection<Person>> GetPeopleAsync(CancellationToken ct = default);

    /// <summary>
    ///     Gets a single person or null if one is not found with that ID.
    /// </summary>
    ValueTask<Person?> GetPersonAsync(ForecastPersonId id, CancellationToken ct = default);

    /// <summary>
    ///     Gets all of the placeholders.
    /// </summary>
    ValueTask<IReadOnlyCollection<Placeholder>> GetPlaceholdersAsync(CancellationToken ct = default);

    /// <summary>
    ///     Gets a single placeholder or null if one is not found with that ID.
    /// </summary>
    ValueTask<Placeholder?> GetPlaceholderAsync(ForecastPlaceholderId id, CancellationToken ct = default);

    /// <summary>
    ///     Gets all of the roles.
    /// </summary>
    ValueTask<IReadOnlyCollection<Role>> GetRolesAsync(CancellationToken ct = default);

    /// <summary>
    ///     Gets a single role or null if one is not found with that ID.
    /// </summary>
    ValueTask<Role?> GetRoleAsync(ForecastRoleId id, CancellationToken ct = default);
}
