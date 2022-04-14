using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace HarvestForecast.Client.Test;

public class RoleTests
{
    [ Fact ]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var roles = await client.GetRolesAsync();

        Assert.NotNull( roles );
        Assert.NotEmpty( roles );

        var first = roles.First();
        Assert.Equal( 1111, first.Id );
        Assert.Equal( "Testing", first.Name );
        Assert.Null( first.HarvestRoleId );
        Assert.Contains( 4374, first.PlaceholderIds );
        Assert.Contains( 488331, first.PlaceholderIds );
        Assert.Contains( 503323704, first.PersonIds );
    }

    [ Fact ]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>( async () => await client.GetRolesAsync() );
    }
}
