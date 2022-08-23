using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HarvestForecast.Client.Test;

public class MilestoneTests
{
    [ Fact ]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var milestones = await client.GetMilestonesAsync();

        Assert.NotNull( milestones );
        Assert.NotEmpty( milestones );

        var first = milestones.First();
        Assert.Equal( 1100000, first.Id );
        Assert.Equal( "Target Start Date", first.Name );
        Assert.Equal( new DateOnly( 2017, 10, 23 ), first.Date );
        Assert.Equal( DateTime.Parse( "2017-08-17T17:28:13.055Z" ), first.UpdatedAt );
        Assert.Equal( 111111, first.UpdatedById );
        Assert.Equal( 2222222, first.ProjectId );
    }

    [ Fact ]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>( async () => await client.GetMilestonesAsync() );
    }
}
