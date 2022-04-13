using System.IO;
using System.Net;
using RichardSzalay.MockHttp;

namespace HarvestForecast.Client.Test;

public static class ApiTestHelper
{
    public static IForecastClient GetMockedForecastClient()
    {
        var handler = new MockHttpMessageHandler();

        handler.AddTestResponse( "whoami", "whoami.json" )
               .AddTestResponse( "account*", "accounts.json" )
               .AddTestResponse( "assignments*", "assignments.json" )
               .AddTestResponse( "clients", "clients.json" );

        var httpClient = handler.ToHttpClient();

        return new ForecastClient( httpClient, GenerateTestOptions() );
    }
    
    public static IForecastClient GetFailedRequestForecastClient()
    {
        var handler = new MockHttpMessageHandler();
        
        handler.When("*").Respond(HttpStatusCode.BadRequest);
        
        var httpClient = handler.ToHttpClient();

        return new ForecastClient( httpClient, GenerateTestOptions() );
    }

    private static MockHttpMessageHandler AddTestResponse( this MockHttpMessageHandler handler, string path, string filename )
    {
        path = ForecastClient.BaseUrl + "/" + path;
        handler.When(path)
               .Respond("application/json", _ => LoadTestContent(filename));
        
        return handler;
    }

    private static ForecastOptions GenerateTestOptions()
    {
        return new ForecastOptions("00000", "test-token");
    }
    
    private static Stream LoadTestContent( string filename )
    {
        return File.OpenRead("./testdata/" + filename);
    }
}
