using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HarvestForecast.Client;
using HarvestForecast.Client.Entities;
using Microsoft.Extensions.Caching.Memory;
using Spectre.Console;

namespace Forecast.Viewer;

public static class Program
{
    private static readonly IMemoryCache cache = new MemoryCache( new MemoryCacheOptions() );

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
        var user = await client.WhoAmIAsync().WrapWithAnsiStatus( "Loading user info..." );
        AnsiConsole.MarkupLine( "[green]We've checked your access and everything looks good![/]" );
        AnsiConsole.WriteLine( $"Your user ID is '{user.Id}'" );

        // Check the account
        var account = await client.GetAccountAsync().WrapWithAnsiStatus( "Loading account info..." );
        AnsiConsole.WriteLine( $"Your account is called '{account.Name}'" );

        if ( !string.IsNullOrEmpty( account.HarvestName ) )
        {
            AnsiConsole.WriteLine( $"Your account is connected to Harvest: '{account.HarvestName}'" );
        }

        // Give a proper welcome
        var person = await client.GetPersonAsync( user.Id );
        if ( person is { } )
        {
            AnsiConsole.Markup( $"[green]Hi {person.FirstName}, we checked your profile details [/] " );
            bool workingToday = person.WorkingDays.IsActiveOn( DateTime.Today.DayOfWeek );
            string workingLine = workingToday ? "and it looks like today is one of your working days!" : "and it looks like today is not one of your working days!";
            AnsiConsole.MarkupLine( $"[green]{workingLine}[/]" );
        }
        else
        {
            AnsiConsole.MarkupLine( "[red]We can't check your profile details right now but we can still check your assignments...[/]" );
        }

        // Check today's assignments
        AnsiConsole.WriteLine();
        var assignments = await client.GetAssignmentsAsync( AssignmentFilter.Today() with {PersonId = user.Id} )
                                      .WrapWithAnsiStatus( "Loading assignments..." );
        if ( assignments.Count == 0 )
        {
            AnsiConsole.MarkupLine( "[green]Hooray! Looks like you've got nothing assigned to you today.[/]" );
        }
        else
        {
            AnsiConsole.MarkupLine( $"[green]Looks like you've got {assignments.Count} assignment{( assignments.Count == 1 ? "" : "s" )}:[/]" );

            var table = new Table();
            table.AddColumn( "Client" );
            table.AddColumn( "Project" );
            table.AddColumn( "Duration" );
            table.AddColumn( "Notes" );

            foreach ( var assignment in assignments )
            {
                var project = await GetProjectAsync( client, assignment.ProjectId );
                var projectClient = project.ClientId.HasValue ? await GetClientAsync( client, project.ClientId.Value ) : null;
                string duration = assignment.Allocation is null ? "-" : assignment.Allocation.Value.TotalHours.ToString( "0.#" ) + "h";
                
                table.AddRow( projectClient?.Name ?? "-",
                              project.Name,
                              duration,
                              assignment.Notes ?? string.Empty );
            }

            AnsiConsole.Write( table );

            AnsiConsole.WriteLine();

            var milestoneTasks = assignments.Select( a => a.ProjectId )
                                            .Distinct()
                                            .Select( p => client.GetMilestonesAsync( new MilestoneFilter {ProjectId = p} ).AsTask() );

            var today = DateOnly.FromDateTime(DateTime.Today);
            var milestoneLimit = today.AddDays(14);
            var milestones = ( await Task.WhenAll( milestoneTasks ) )
                            .SelectMany( m => m )
                            .Where( m => m.Date >= today && m.Date <= milestoneLimit )
                            .ToList();

            if ( milestones.Any() )
            {
                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine( "[green]Upcoming milestones:[/]" );

                table = new Table();
                table.AddColumn( "Client" );
                table.AddColumn( "Project" );
                table.AddColumn( "Date" );
                table.AddColumn( "Milestone" );

                foreach ( var milestone in milestones )
                {
                    var project = await GetProjectAsync( client, milestone.ProjectId );
                    var projectClient = project.ClientId.HasValue ? await GetClientAsync( client, project.ClientId.Value ) : null;

                    table.AddRow( projectClient?.Name ?? "-",
                                  project.Name,
                                  milestone.Date.ToString( "d" ),
                                  milestone.Name );
                }

                AnsiConsole.Write( table );
            }
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

        return await cache.GetOrCreateAsync( key, async _ => await forecastClient.GetClientAsync( id ) ) ?? throw new InvalidOperationException( "Failed to fetch the specified client" );
    }

    private static async ValueTask<Project> GetProjectAsync( IForecastClient forecastClient, int id )
    {
        string key = $"project:{id}";

        return await cache.GetOrCreateAsync( key, async _ => await forecastClient.GetProjectAsync( id ) ) ?? throw new InvalidOperationException( "Failed to fetch the specified client" );
    }

    private static async ValueTask<T> WrapWithAnsiStatus<T>( this ValueTask<T> task, string status )
    {
        var result = await AnsiConsole.Status()
                                      .StartAsync( status, async context =>
                                                           {
                                                               context.Spinner = Spinner.Known.Circle;
                                                               context.SpinnerStyle = Style.Parse( "fuchsia" );

                                                               return await task;
                                                           } );

        return result;
    }
}
