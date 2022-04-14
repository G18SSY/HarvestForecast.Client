using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HarvestForecast.Client.Test;

public class PlaceholderTests
{
    [ Fact ]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var placeholders = await client.GetPlaceholderAsync();

        Assert.NotNull( placeholders );
        Assert.NotEmpty( placeholders );

        var first = placeholders.First();
        Assert.Equal( 1, first.Id );
        Assert.Equal( "Test Placeholder", first.Name );
        Assert.False( first.Archived );
        Assert.Equal( DateTime.Parse( "2018-01-09T20:40:24.000Z" ), first.UpdatedAt );
        Assert.Equal( 1, first.UpdatedById );
        Assert.Contains( "role 2", first.Roles );
    }

    [ Fact ]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>( async () => await client.GetPlaceholderAsync() );
    }
}
