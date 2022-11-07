using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HarvestForecast.Client.Entities.VO;
using Xunit;

namespace HarvestForecast.Client.Test;

public class PlaceholderTests
{
    [Fact]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var placeholders = await client.GetPlaceholdersAsync();

        Assert.NotNull(placeholders);
        Assert.NotEmpty(placeholders);

        var first = placeholders.First();
        Assert.Equal(ForecastPlaceholderId.From(1), first.Id);
        Assert.Equal("Test Placeholder", first.Name);
        Assert.False(first.Archived);
        Assert.Equal(DateTime.Parse("2018-01-09T20:40:24.000Z"), first.UpdatedAt);
        Assert.Equal(ForecastPersonId.From(1), first.UpdatedById);
        Assert.Contains(ForecastRoleName.From("role 2"), first.Roles);
    }

    [Fact]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>(async () => await client.GetPlaceholdersAsync());
    }
}
