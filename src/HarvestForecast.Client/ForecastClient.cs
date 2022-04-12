using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace HarvestForecast.Client;

public class ForecastClient : IForecastClient
{
    internal const string BaseUrl = "https://api.forecastapp.com";

    private readonly HttpClient httpClient;
    private readonly ForecastOptions options;

    public ForecastClient( HttpClient httpClient, ForecastOptions options )
    {
        this.httpClient = httpClient;
        this.options = options;
    }

    public ValueTask<CurrentUser> WhoAmIAsync()
    {
        return GetEntityAsync<CurrentUser>( "whoami" );
    }

    protected virtual ValueTask AuthenticateRequest( HttpRequestMessage request )
    {
        request.Headers.Authorization = new AuthenticationHeaderValue( "Bearer", options.AccessToken );
        request.Headers.Add( "Forecast-Account-ID", options.AccountId );

        return new ValueTask();
    }

    private HttpRequestMessage GetRequestMessage( string subPath )
    {
        string path = BaseUrl + "/" + subPath;
        var uri = new Uri( path, UriKind.Absolute );

        return new HttpRequestMessage( HttpMethod.Get, uri );
    }

    private async ValueTask<T> GetEntityAsync<T>( string subPath )
    {
        string containerPropertyName = GetContainerPropertyName<T>();

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

    private static string GetContainerPropertyName<T>()
    {
        var type = typeof( T );
        var attribute = type.GetCustomAttribute<ContainerPropertyAttribute>();

        if ( attribute is null )
        {
            throw new InvalidOperationException( $"{type.Name} does not define a container name" );
        }

        return attribute.Name;
    }
}
