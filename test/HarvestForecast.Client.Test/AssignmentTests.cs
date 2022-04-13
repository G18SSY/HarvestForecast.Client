using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HarvestForecast.Client.Entities;
using Xunit;

namespace HarvestForecast.Client.Test;

public class AssignmentTests
{
    [ Fact ]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var assignments = await client.Assignments( AssignmentFilter.Today() );

        Assert.NotNull( assignments );
        Assert.NotEmpty( assignments );

        var first = assignments.First();
        Assert.Equal( 1000001, first.Id );
        Assert.Equal( new DateTime( 2017, 05, 24 ), first.StartDate );
        Assert.Equal( new DateTime( 2017, 05, 29 ), first.EndDate );
        Assert.Null( first.Allocation );
        Assert.Null( first.Notes );
        Assert.Equal( DateTime.Parse( "2017-05-02T19:07:00.478Z" ), first.UpdatedAt );
        Assert.Equal( 111111, first.UpdatedById );
        Assert.Equal( 222222, first.ProjectId );
        Assert.Equal( 333333, first.PersonId );
        Assert.Null( first.PlaceholderId );
        Assert.Null( first.RepeatedAssignmentSetId );
    }

    // TODO Add a test for the filters
    // TODO Test query params in filter

    [ Fact ]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>( async () => await client.Assignments( AssignmentFilter.Today() ) );
    }
}
