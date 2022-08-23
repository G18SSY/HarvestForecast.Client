using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HarvestForecast.Client.Test;

public class ProjectTests
{
    [ Fact ]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var projects = await client.GetProjectsAsync();

        Assert.NotNull( projects );
        Assert.NotEmpty( projects );

        var first = projects.First();
        Assert.Equal( 111111, first.Id );
        Assert.Equal( "Test Project 1", first.Name );
        Assert.Equal( "black", first.Color );
        Assert.Null( first.Code );
        Assert.Null( first.Notes );
        Assert.Equal( new DateOnly( 2017, 01, 05 ), first.StartDate );
        Assert.Equal( new DateOnly( 2018, 02, 23 ), first.EndDate );
        Assert.Null( first.HarvestId );
        Assert.False( first.Archived );
        Assert.Equal( DateTime.Parse( "2017-10-13T20:49:36.418Z" ), first.UpdatedAt );
        Assert.Equal( 111111, first.UpdatedById );
        Assert.Null( first.ClientId );
        Assert.Empty( first.Tags );

        var second = projects.Skip( 1 ).First();
        var secondTags = second.Tags;
        Assert.Contains( "test", secondTags );
        Assert.Contains( "project", secondTags );
    }

    [ Fact ]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>( async () => await client.GetProjectsAsync() );
    }
}
