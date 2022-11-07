using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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
    public ValueTask<CurrentUser> WhoAmIAsync() => GetEntityAsync<CurrentUser>("whoami", "current_user");

    /// <inheritdoc />
    public ValueTask<Account> GetAccountAsync() => GetEntityAsync<Account>($"accounts/{options.AccountId}", "account");

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Assignment>> GetAssignmentsAsync(AssignmentFilter filter) =>
        GetEntityAsync<IReadOnlyCollection<Assignment>>("assignments", "assignments", filter);

    /// <inheritdoc />
    public ValueTask<Assignment> CreateAssignmentAsync(AssignmentData assignment) =>
        CreateEntityAsync<AssignmentData, Assignment>("assignments", "assignment", assignment);

    /// <inheritdoc />
    public ValueTask<Assignment> UpdateAssignmentAsync(ForecastAssignmentId id, AssignmentData assignment) =>
        UpdateEntityAsync<AssignmentData, Assignment>("assignments", "assignment", id.Value, assignment);

    /// <inheritdoc />
    public ValueTask<bool> RemoveAssignmentAsync(ForecastAssignmentId id) => RemoveEntityAsync("assignments", id.Value);

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Project>> GetProjectsAsync() =>
        GetEntityAsync<IReadOnlyCollection<Project>>("projects", "projects");

    /// <inheritdoc />
    public ValueTask<Project?> GetProjectAsync(ForecastProjectId id) =>
        GetEntityAsync<Project>($"projects/{id}", "project")
            .ReturnNullIfNotFoundAsync();

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Entities.Client>> GetClientsAsync() =>
        GetEntityAsync<IReadOnlyCollection<Entities.Client>>("clients", "clients");

    /// <inheritdoc />
    public ValueTask<Entities.Client?> GetClientAsync(ForecastClientId id) =>
        GetEntityAsync<Entities.Client>($"clients/{id}", "client")
            .ReturnNullIfNotFoundAsync();

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Milestone>> GetMilestonesAsync() => GetMilestonesAsync(MilestoneFilter.None);

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Milestone>> GetMilestonesAsync(MilestoneFilter filter) =>
        GetEntityAsync<IReadOnlyCollection<Milestone>>("milestones", "milestones", filter);

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Person>> GetPeopleAsync() =>
        GetEntityAsync<IReadOnlyCollection<Person>>("people", "people");

    /// <inheritdoc />
    public ValueTask<Person?> GetPersonAsync(ForecastPersonId id) =>
        GetEntityAsync<Person>($"people/{id}", "person")
            .ReturnNullIfNotFoundAsync();

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Placeholder>> GetPlaceholdersAsync() =>
        GetEntityAsync<IReadOnlyCollection<Placeholder>>("placeholders", "placeholders");

    /// <inheritdoc />
    public ValueTask<Placeholder?> GetPlaceholderAsync(ForecastPlaceholderId id) =>
        GetEntityAsync<Placeholder>($"placeholders/{id}", "placeholder")
            .ReturnNullIfNotFoundAsync();

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Role>> GetRolesAsync() =>
        GetEntityAsync<IReadOnlyCollection<Role>>("roles", "roles");

    /// <inheritdoc />
    public ValueTask<Role?> GetRoleAsync(ForecastRoleId id) =>
        GetEntityAsync<Role>($"roles/{id}", "role")
            .ReturnNullIfNotFoundAsync();

    /// <summary>
    ///     Adds authentication headers to a request. This is called before the request is sent and can be extended to add
    ///     additional headers in an overriding class.
    /// </summary>
    protected virtual ValueTask AuthenticateRequest(HttpRequestMessage request)
    {
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", options.AccessToken);
        request.Headers.Add("Forecast-Account-ID", options.AccountId);

        return new ValueTask();
    }

    private async ValueTask<HttpRequestMessage> CreateRequestAsync(string subPath, HttpMethod method)
    {
        var path = BaseUrl + "/" + subPath;
        var uri = new Uri(path, UriKind.Absolute);

        var request = new HttpRequestMessage(method, uri);
        await AuthenticateRequest(request);

        return request;
    }

    private async ValueTask<T> GetEntityAsync<T>(string subPath, string containerPropertyName,
        FilterBase? filter = null)
    {
        if (filter is { })
        {
            var queryString = filter.GetFilterQuery();

            if (!string.IsNullOrEmpty(queryString)) subPath += $"?{queryString}";
        }

        var request = await CreateRequestAsync(subPath, HttpMethod.Get);
        var response = await SendRequestAsync(request);

        return await NestedJsonHelper.UnwrapNestedJsonContentAsync<T>(response.Content, containerPropertyName);
    }

    private async ValueTask<TResponse> CreateEntityAsync<TData, TResponse>(string subPath, string containerPropertyName,
        TData data)
    {
        var request = await CreateRequestAsync(subPath, HttpMethod.Post);
        request.Content = NestedJsonHelper.CreateNestedJsonContent(data, containerPropertyName);
        var response = await SendRequestAsync(request);

        return await NestedJsonHelper.UnwrapNestedJsonContentAsync<TResponse>(response.Content, containerPropertyName);
    }

    private async ValueTask<TResponse> UpdateEntityAsync<TData, TResponse>(string subPath, string containerPropertyName,
        int id, TData data)
    {
        var request = await CreateRequestAsync($"{subPath}/{id}", HttpMethod.Put);
        request.Content = NestedJsonHelper.CreateNestedJsonContent(data, containerPropertyName);
        var response = await SendRequestAsync(request);

        return await NestedJsonHelper.UnwrapNestedJsonContentAsync<TResponse>(response.Content, containerPropertyName);
    }

    private async ValueTask<bool> RemoveEntityAsync(string subPath, int id)
    {
        var request = await CreateRequestAsync($"{subPath}/{id}", HttpMethod.Delete);
        try
        {
            var response = await SendRequestAsync(request);
            Debug.Assert(response.StatusCode == HttpStatusCode.NoContent);
            return true;
        }
        catch (NotFoundException)
        {
            return false;
        }
    }

    private async ValueTask<HttpResponseMessage> SendRequestAsync(HttpRequestMessage request)
    {
        var response = await httpClient.SendAsync(request);

        // Throw a special exception for 404 so we can filter out these results
        if (response.StatusCode == HttpStatusCode.NotFound) throw new NotFoundException();

        response.EnsureSuccessStatusCode();

        return response;
    }
}