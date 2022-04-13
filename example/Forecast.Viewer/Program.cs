using HarvestForecast.Client;
using HarvestForecast.Client.Entities;
using Microsoft.Extensions.Caching.Memory;
using Spectre.Console;

namespace Forecast.Viewer;

public static class Program
{
    private static readonly IMemoryCache Cache = new MemoryCache( new MemoryCacheOptions() );
    
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

        // Check the account
        var account = await client.Account();
        AnsiConsole.WriteLine( $"Your account is called '{account.Name}'" );

        if ( !string.IsNullOrEmpty( account.HarvestName ) )
            AnsiConsole.WriteLine( $"Your account is connected to Harvest: '{account.HarvestName}'" );
        
        // Check today's assignments
        AnsiConsole.WriteLine();
        var assignments = await client.Assignments( AssignmentFilter.Today() with {PersonId = user.Id} );
        if ( assignments.Count == 0 )
        {
            AnsiConsole.MarkupLine("[green]Hooray! Looks like you've got nothing assigned to you today.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine( $"[green]Looks like you've got {assignments.Count} assignment{( assignments.Count == 1 ? "" : "s" )}:[/]" );

            var table = new Table();
            table.AddColumn( "Client" );
            table.AddColumn( "Project" );
            table.AddColumn( "Notes" );

            foreach ( var assignment in assignments )
            {
                var project = await GetProjectAsync( client, assignment.ProjectId );
                var projectClient = project.ClientId.HasValue ? await GetClientAsync( client, project.ClientId.Value ) : null;

                table.AddRow( projectClient?.Name ?? "-", project.Name, assignment.Notes ?? string.Empty );
            }
            
            AnsiConsole.Write( table );
        }
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

    private static async ValueTask<Client> GetClientAsync( IForecastClient forecastClient, int id )
    {
        string key = $"client:{id}";

        return await Cache.GetOrCreateAsync( key, async _ => await forecastClient.Client( id ) );
    }
    
    private static async ValueTask<Project> GetProjectAsync( IForecastClient forecastClient, int id )
    {
        string key = $"project:{id}";

        return await Cache.GetOrCreateAsync( key, async _ => await forecastClient.Project(id) );
    }
}
