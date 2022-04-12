using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace HarvestForecast.Client;

public class ForecastClient : IForecastClient
{
	internal const string BaseUrl = "https://api.forecastapp.com";
    
    private readonly HttpClient httpClient;
    private readonly ForecastOptions options;
    
    public ForecastClient( HttpClient httpClient, ForecastOptions options)
    {
	    this.httpClient = httpClient;
	    this.options = options;
    }

    protected virtual ValueTask AuthenticateRequest( HttpRequestMessage request )
    {
	    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue( "Bearer", options.AccessToken );
	    request.Headers.Add( "Forecast-Account-ID", options.AccountId );

	    return new ValueTask();
    }

    private HttpRequestMessage GetRequestMessage( string subPath )
    {
	    string path = BaseUrl + "/" + subPath;
	    var uri = new Uri( path, UriKind.Absolute );

	    return new HttpRequestMessage( HttpMethod.Get, uri );
    }

    public async ValueTask<CurrentUser> WhoAmIAsync()
    {
	    var request = GetRequestMessage( "whoami" );
	    await AuthenticateRequest( request );

	    var response = await httpClient.SendAsync( request );
	    response.EnsureSuccessStatusCode();

	    using var content = await response.Content.ReadAsStreamAsync();
	    var container = await JsonSerializer.DeserializeAsync<CurrentUserContainer>( content );

	    if ( container is null )
		    throw new InvalidOperationException( "Unable to deserialize response" );

	    return container.CurrentUser;
    }
}
