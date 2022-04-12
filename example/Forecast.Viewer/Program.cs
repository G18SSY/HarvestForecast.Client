using HarvestForecast.Client;
using Spectre.Console;

namespace Forecast.Viewer;

public static class Program
{
    public static async Task Main()
    {
        if ( !SayHello() )
        {
            return;
        }

        var options = LoadOptions();
        if ( options is null )
        {
            return;
        }

        var client = new ForecastClient( new HttpClient(), options );

        // Detect the user
        var user = await client.WhoAmIAsync();

        AnsiConsole.MarkupLine( "[green]We've checked your access and everything looks good![/]" );
        AnsiConsole.WriteLine( $"Your user ID is '{user.Id}'" );
    }

    private static bool SayHello()
    {
        bool confirmed = AnsiConsole.Confirm( "[#F77726]Hello! Would you like to see the Forecast?[/]" );

        AnsiConsole.WriteLine();

        return confirmed;
    }

    private static ForecastOptions? LoadOptions()
    {
        var id = AnsiConsole.Ask<string>( "[blue] What's your account ID?[/]" );

        if ( string.IsNullOrEmpty( id ) )
        {
            return null;
        }

        var token = AnsiConsole.Ask<string>( "[blue] What's your access token?[/]" );

        if ( string.IsNullOrEmpty( token ) )
        {
            return null;
        }

        AnsiConsole.WriteLine();

        return new ForecastOptions( id, token );
    }
}
