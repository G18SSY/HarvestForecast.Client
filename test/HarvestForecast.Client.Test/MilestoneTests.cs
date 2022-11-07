using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HarvestForecast.Client.Entities.VO;
using Xunit;

namespace HarvestForecast.Client.Test;

public class MilestoneTests
{
    [Fact]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var milestones = await client.GetMilestonesAsync();

        Assert.NotNull(milestones);
        Assert.NotEmpty(milestones);

        var first = milestones.First();
        Assert.Equal(ForecastMilestoneId.From(1100000), first.Id);
        Assert.Equal("Target Start Date", first.Name);
        Assert.Equal(new DateOnly(2017, 10, 23), first.Date);
        Assert.Equal(DateTime.Parse("2017-08-17T17:28:13.055Z"), first.UpdatedAt);
        Assert.Equal(ForecastPersonId.From(111111), first.UpdatedById);
        Assert.Equal(ForecastProjectId.From(2222222), first.ProjectId);
    }

    [Fact]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>(async () => await client.GetMilestonesAsync());
    }
}
