using System.Net.Http;
using System.Threading.Tasks;
using HarvestForecast.Client.Entities.VO;
using Xunit;

namespace HarvestForecast.Client.Test;

public class WhoAmITests
{
    [Fact]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var user = await client.WhoAmIAsync();

        Assert.NotNull(user);
        Assert.Equal(ForecastPersonId.From(123456), user.Id);
        Assert.Equal(2, user.AccountIds.Count);
        Assert.Contains(ForecastAccountId.From(111111), user.AccountIds);
        Assert.Contains(ForecastAccountId.From(222222), user.AccountIds);
    }

    [Fact]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>(async () => await client.WhoAmIAsync());
    }
}
