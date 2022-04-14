using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HarvestForecast.Client.Test;

public class ClientTests
{
    [ Fact ]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var clients = await client.GetClientsAsync();

        Assert.NotNull( clients );
        Assert.NotEmpty( clients );

        var first = clients.First();
        Assert.Equal( 1234, first.Id );
        Assert.Equal( "The Boss", first.Name );
        Assert.Equal( 456, first.HarvestId );
        Assert.False( first.Archived );
        Assert.Equal( DateTime.Parse( "2015-07-27T07:17:11.801Z" ), first.UpdatedAt );
        Assert.Equal( 2324, first.UpdatedById );
    }

    [ Fact ]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>( async () => await client.GetClientsAsync() );
    }
}
