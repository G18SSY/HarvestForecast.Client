using System;
using System.Net.Http;
using System.Threading.Tasks;
using HarvestForecast.Client.Entities;
using HarvestForecast.Client.Entities.VO;
using Xunit;

namespace HarvestForecast.Client.Test;

public class AccountTests
{
    [Fact]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var account = await client.GetAccountAsync();

        Assert.NotNull(account);
        Assert.Equal(ForecastAccountId.From(987654), account.Id);
        Assert.Equal("Test", account.Name);
        Assert.Equal(TimeSpan.FromHours(40), account.WeeklyCapacity);
        Assert.Equal("Test", account.HarvestName);
        Assert.Equal("test", account.HarvestSubDomain);
        Assert.Equal(8, account.ColorLabels.Count);
        Assert.Contains(new ColorLabel("aqua", "Penciled"), account.ColorLabels);
        Assert.Contains(new ColorLabel("gray", "Done"), account.ColorLabels);
    }

    [Fact]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>(async () => await client.GetAccountAsync());
    }
}
