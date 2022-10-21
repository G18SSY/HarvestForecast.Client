using System.IO;
using System.Net;
using RichardSzalay.MockHttp;

namespace HarvestForecast.Client.Test;

public static class ApiTestHelper
{
    public static readonly ForecastOptions TestOptions = new("00000", "test-token");

    public static IForecastClient GetMockedForecastClient()
    {
        var handler = new MockHttpMessageHandler();

        handler.AddTestResponse( "whoami", "whoami.json" )
               .AddTestResponse( "account*", "accounts.json" )
               .AddTestResponse( "assignments*", "assignments.json" )
               .AddTestResponse( "projects", "projects.json" )
               .AddTestResponse( "milestones", "milestones.json" )
               .AddTestResponse( "placeholders", "placeholders.json" )
               .AddTestResponse( "roles", "roles.json" )
               .AddTestResponse( "people", "people.json" )
               .AddTestResponse( "clients", "clients.json" );

        var httpClient = handler.ToHttpClient();

        return new ForecastClient( httpClient, TestOptions );
    }

    public static IForecastClient GetFailedRequestForecastClient()
    {
        var handler = new MockHttpMessageHandler();
        
        handler.When("*").Respond(HttpStatusCode.BadRequest);
        
        var httpClient = handler.ToHttpClient();

        return new ForecastClient( httpClient, TestOptions );
    }

    public static string GetFullPath(string path)
        => ForecastClient.BaseUrl + "/" + path;

    public static MockedRequest RespondWithJsonTestData(this MockedRequest request, string filename)
        => request.Respond("application/json", _ => LoadTestContent(filename));

    private static MockHttpMessageHandler AddTestResponse( this MockHttpMessageHandler handler, string path, string filename )
    {
        path = GetFullPath(path);
        handler.When(path)
            .RespondWithJsonTestData(filename);

        return handler;
    }


    private static Stream LoadTestContent( string filename )
    {
        return File.OpenRead("./testdata/" + filename);
    }
}
