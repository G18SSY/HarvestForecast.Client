using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HarvestForecast.Client.Test;

public class WhoAmITests
{
    [ Fact ]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var user = await client.WhoAmIAsync();

        Assert.NotNull( user );
        Assert.Equal( 123456, user.Id );
        Assert.Equal( 2, user.AccountIds.Count );
        Assert.Contains( 111111, user.AccountIds );
        Assert.Contains( 222222, user.AccountIds );
    }

    [ Fact ]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>( async () => await client.WhoAmIAsync() );
    }
}
