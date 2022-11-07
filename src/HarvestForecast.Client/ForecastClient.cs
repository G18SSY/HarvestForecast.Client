using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using HarvestForecast.Client.Entities;
using HarvestForecast.Client.Entities.VO;

namespace HarvestForecast.Client;

/// <summary>
///     An implementation of <see cref="IForecastClient" /> for fetching data from Harvest Forecast.
/// </summary>
public class ForecastClient : IForecastClient
{
    internal const string BaseUrl = "https://api.forecastapp.com";

    private readonly HttpClient httpClient;
    private readonly ForecastOptions options;

    /// <summary>
    ///     Creates a new client.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient" /> instance to use for sending requests.</param>
    /// <param name="options">The auth options.</param>
    public ForecastClient(HttpClient httpClient, ForecastOptions options)
    {
        this.httpClient = httpClient;
        this.options = options;
    }

    /// <inheritdoc />
    public ValueTask<CurrentUser> WhoAmIAsync(CancellationToken ct = default) => 
        GetEntityAsync<CurrentUser>("whoami", "current_user", ct);

    /// <inheritdoc />
    public ValueTask<Account> GetAccountAsync(CancellationToken ct = default) => 
        GetEntityAsync<Account>($"accounts/{options.AccountId}", "account", ct);

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Assignment>> GetAssignmentsAsync(AssignmentFilter filter, CancellationToken ct = default) =>
        GetEntityAsync<IReadOnlyCollection<Assignment>>("assignments", "assignments", ct, filter);

    /// <inheritdoc />
    public ValueTask<Assignment> CreateAssignmentAsync(AssignmentData assignment, CancellationToken ct = default) =>
        CreateEntityAsync<AssignmentData, Assignment>("assignments", "assignment", assignment, ct);

    /// <inheritdoc />
    public ValueTask<Assignment> UpdateAssignmentAsync(ForecastAssignmentId id, AssignmentData assignment, CancellationToken ct = default) =>
        UpdateEntityAsync<AssignmentData, Assignment>("assignments", "assignment", id.Value, assignment, ct);

    /// <inheritdoc />
    public ValueTask<bool> RemoveAssignmentAsync(ForecastAssignmentId id, CancellationToken ct = default) =>
        RemoveEntityAsync("assignments", id.Value, ct);

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Project>> GetProjectsAsync(CancellationToken ct = default) =>
        GetEntityAsync<IReadOnlyCollection<Project>>("projects", "projects", ct);

    /// <inheritdoc />
    public ValueTask<Project?> GetProjectAsync(ForecastProjectId id, CancellationToken ct = default) =>
        GetEntityAsync<Project>($"projects/{id}", "project", ct)
            .ReturnNullIfNotFoundAsync();

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Entities.Client>> GetClientsAsync(CancellationToken ct = default) =>
        GetEntityAsync<IReadOnlyCollection<Entities.Client>>("clients", "clients", ct);

    /// <inheritdoc />
    public ValueTask<Entities.Client?> GetClientAsync(ForecastClientId id, CancellationToken ct = default) =>
        GetEntityAsync<Entities.Client>($"clients/{id}", "client", ct)
            .ReturnNullIfNotFoundAsync();

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Milestone>> GetMilestonesAsync(CancellationToken ct = default) =>
        GetMilestonesAsync(MilestoneFilter.None, ct);

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Milestone>> GetMilestonesAsync(MilestoneFilter filter, CancellationToken ct = default) =>
        GetEntityAsync<IReadOnlyCollection<Milestone>>("milestones", "milestones", ct, filter);

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Person>> GetPeopleAsync(CancellationToken ct = default) =>
        GetEntityAsync<IReadOnlyCollection<Person>>("people", "people", ct);

    /// <inheritdoc />
    public ValueTask<Person?> GetPersonAsync(ForecastPersonId id, CancellationToken ct = default) =>
        GetEntityAsync<Person>($"people/{id}", "person", ct)
            .ReturnNullIfNotFoundAsync();

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Placeholder>> GetPlaceholdersAsync(CancellationToken ct = default) =>
        GetEntityAsync<IReadOnlyCollection<Placeholder>>("placeholders", "placeholders", ct);

    /// <inheritdoc />
    public ValueTask<Placeholder?> GetPlaceholderAsync(ForecastPlaceholderId id, CancellationToken ct = default) =>
        GetEntityAsync<Placeholder>($"placeholders/{id}", "placeholder", ct)
            .ReturnNullIfNotFoundAsync();

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Role>> GetRolesAsync(CancellationToken ct = default) =>
        GetEntityAsync<IReadOnlyCollection<Role>>("roles", "roles", ct);

    /// <inheritdoc />
    public ValueTask<Role?> GetRoleAsync(ForecastRoleId id, CancellationToken ct = default) =>
        GetEntityAsync<Role>($"roles/{id}", "role", ct)
            .ReturnNullIfNotFoundAsync();

    /// <summary>
    ///     Adds authentication headers to a request. This is called before the request is sent and can be extended to add
    ///     additional headers in an overriding class.
    /// </summary>
    protected virtual ValueTask AuthenticateRequest(HttpRequestMessage request, CancellationToken ct)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.AccessToken);
        request.Headers.Add("Forecast-Account-ID", options.AccountId);

        return new ValueTask();
    }

    private async ValueTask<HttpRequestMessage> CreateRequestAsync(string subPath, HttpMethod method, CancellationToken ct)
    {
        var path = BaseUrl + "/" + subPath;
        var uri = new Uri(path, UriKind.Absolute);

        var request = new HttpRequestMessage(method, uri);
        await AuthenticateRequest(request, ct);

        return request;
    }

    private async ValueTask<T> GetEntityAsync<T>(string subPath, string containerPropertyName,
        CancellationToken ct, FilterBase? filter = null)
    {
        if (filter is { })
        {
            var queryString = filter.GetFilterQuery();

            if (!string.IsNullOrEmpty(queryString)) subPath += $"?{queryString}";
        }

        var request = await CreateRequestAsync(subPath, HttpMethod.Get, ct);
        var response = await SendRequestAsync(request, ct);

        return await NestedJsonHelper.UnwrapNestedJsonContentAsync<T>(response.Content, containerPropertyName);
    }

    private async ValueTask<TResponse> CreateEntityAsync<TData, TResponse>(string subPath, string containerPropertyName,
        TData data, CancellationToken ct)
    {
        var request = await CreateRequestAsync(subPath, HttpMethod.Post, ct);
        request.Content = NestedJsonHelper.CreateNestedJsonContent(data, containerPropertyName);
        var response = await SendRequestAsync(request, ct);

        return await NestedJsonHelper.UnwrapNestedJsonContentAsync<TResponse>(response.Content, containerPropertyName);
    }

    private async ValueTask<TResponse> UpdateEntityAsync<TData, TResponse>(string subPath, string containerPropertyName,
        int id, TData data, CancellationToken ct)
    {
        var request = await CreateRequestAsync($"{subPath}/{id}", HttpMethod.Put, ct);
        request.Content = NestedJsonHelper.CreateNestedJsonContent(data, containerPropertyName);
        var response = await SendRequestAsync(request, ct);

        return await NestedJsonHelper.UnwrapNestedJsonContentAsync<TResponse>(response.Content, containerPropertyName);
    }

    private async ValueTask<bool> RemoveEntityAsync(string subPath, int id, CancellationToken ct)
    {
        var request = await CreateRequestAsync($"{subPath}/{id}", HttpMethod.Delete, ct);
        try
        {
            var response = await SendRequestAsync(request, ct);
            Debug.Assert(response.StatusCode == HttpStatusCode.NoContent);
            return true;
        }
        catch (NotFoundException)
        {
            return false;
        }
    }

    private async ValueTask<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request, CancellationToken ct)
    {
        var response = await httpClient.SendAsync(request, ct);

        // Throw a special exception for 404 so we can filter out these results
        if (response.StatusCode == HttpStatusCode.NotFound) throw new NotFoundException();

        response.EnsureSuccessStatusCode();

        return response;
    }
}