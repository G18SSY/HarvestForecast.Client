using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using HarvestForecast.Client.Entities;

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
    public ForecastClient( HttpClient httpClient, ForecastOptions options )
    {
        this.httpClient = httpClient;
        this.options = options;
    }

    /// <inheritdoc />
    public ValueTask<CurrentUser> WhoAmIAsync()
    {
        return GetEntityAsync<CurrentUser>( "whoami", "current_user" );
    }

    /// <inheritdoc />
    public ValueTask<Account> GetAccountAsync()
    {
        return GetEntityAsync<Account>( $"accounts/{options.AccountId}", "account" );
    }

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Assignment>> GetAssignmentsAsync( AssignmentFilter filter )
    {
        return GetEntityAsync<IReadOnlyCollection<Assignment>>( "assignments", "assignments", filter );
    }

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Project>> GetProjectsAsync()
    {
        return GetEntityAsync<IReadOnlyCollection<Project>>( "projects", "projects" );
    }

    /// <inheritdoc />
    public ValueTask<Project?> GetProjectAsync( int id )
    {
        return GetEntityAsync<Project>( $"projects/{id}", "project" )
           .ReturnNullIfNotFoundAsync();
    }

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Entities.Client>> GetClientsAsync()
    {
        return GetEntityAsync<IReadOnlyCollection<Entities.Client>>( "clients", "clients" );
    }

    /// <inheritdoc />
    public ValueTask<Entities.Client?> GetClientAsync( int id )
    {
        return GetEntityAsync<Entities.Client>( $"clients/{id}", "client" )
           .ReturnNullIfNotFoundAsync();
    }

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Milestone>> GetMilestonesAsync()
    {
        return GetMilestonesAsync( MilestoneFilter.None );
    }

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Milestone>> GetMilestonesAsync( MilestoneFilter filter )
    {
        return GetEntityAsync<IReadOnlyCollection<Milestone>>( "milestones", "milestones", filter );
    }

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Person>> GetPeopleAsync()
    {
        return GetEntityAsync<IReadOnlyCollection<Person>>( "people", "people" );
    }

    /// <inheritdoc />
    public ValueTask<Person?> GetPersonAsync( int id )
    {
        return GetEntityAsync<Person>( $"people/{id}", "person" )
           .ReturnNullIfNotFoundAsync();
    }

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Placeholder>> GetPlaceholderAsync()
    {
        return GetEntityAsync<IReadOnlyCollection<Placeholder>>( "placeholders", "placeholders" );
    }

    /// <inheritdoc />
    public ValueTask<Placeholder?> GetPlaceholderAsync( int id )
    {
        return GetEntityAsync<Placeholder>( $"placeholders/{id}", "placeholder" )
           .ReturnNullIfNotFoundAsync();
    }

    /// <summary>
    ///     Adds authentication headers to a request. This is called before the request is sent and can be extended to add
    ///     additional headers in an overriding class.
    /// </summary>
    protected virtual ValueTask AuthenticateRequest( HttpRequestMessage request )
    {
        request.Headers.Authorization = new AuthenticationHeaderValue( "Bearer", options.AccessToken );
        request.Headers.Add( "Forecast-Account-ID", options.AccountId );

        return new ValueTask();
    }

    private static HttpRequestMessage GetRequestMessage( string subPath )
    {
        string path = BaseUrl + "/" + subPath;
        var uri = new Uri( path, UriKind.Absolute );

        return new HttpRequestMessage( HttpMethod.Get, uri );
    }

    private async ValueTask<T> GetEntityAsync<T>( string subPath, string containerPropertyName, FilterBase? filter = null )
    {
        if ( filter is { } )
        {
            string queryString = filter.GetFilterQuery();

            if ( !string.IsNullOrEmpty( queryString ) )
            {
                subPath += $"?{queryString}";
            }
        }

        var request = GetRequestMessage( subPath );
        await AuthenticateRequest( request );

        var response = await httpClient.SendAsync( request );

        // Throw a special exception for 404 so we can filter out these results
        if ( response.StatusCode == HttpStatusCode.NotFound )
        {
            throw new NotFoundException();
        }

        response.EnsureSuccessStatusCode();

        using var content = await response.Content.ReadAsStreamAsync();
        var document = await JsonDocument.ParseAsync( content );
        var entity = document.RootElement.GetProperty( containerPropertyName ).Deserialize<T>();

        if ( entity is null )
        {
            throw new InvalidOperationException( "Unable to deserialize response" );
        }

        return entity;
    }
}
