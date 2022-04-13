using System;
using System.Collections.Generic;
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
    public ValueTask<Account> Account()
    {
        return GetEntityAsync<Account>( $"accounts/{options.AccountId}", "account");
    }

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Assignment>> Assignments( AssignmentFilter filter )
    {
        string queryString = filter.GetFilterQuery();
        var subPath = $"assignments?{queryString}";

        return GetEntityAsync<IReadOnlyCollection<Assignment>>( subPath, "assignments" );
    }

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Project>> Projects()
    {
        return GetEntityAsync<IReadOnlyCollection<Project>>( "projects", "projects" );
    }

    /// <inheritdoc />
    public ValueTask<Project> Project( int id )
    {
        return GetEntityAsync<Project>( $"projects/{id}", "project" );
    }

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<Entities.Client>> Clients()
    {
        return GetEntityAsync<IReadOnlyCollection<Entities.Client>>( "clients", "clients" );
    }

    /// <inheritdoc />
    public ValueTask<Entities.Client> Client( int id )
    {
        return GetEntityAsync<Entities.Client>( $"clients/{id}", "client" );
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

    private async ValueTask<T> GetEntityAsync<T>( string subPath, string containerPropertyName )
    {
        var request = GetRequestMessage( subPath );
        await AuthenticateRequest( request );

        var response = await httpClient.SendAsync( request );
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
