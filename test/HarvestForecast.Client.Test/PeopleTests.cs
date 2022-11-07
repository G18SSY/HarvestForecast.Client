using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HarvestForecast.Client.Entities;
using HarvestForecast.Client.Entities.VO;
using Xunit;

namespace HarvestForecast.Client.Test;

public class PeopleTests
{
    [Fact]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var people = await client.GetPeopleAsync();

        Assert.NotNull(people);
        Assert.NotEmpty(people);

        var first = people.First();
        Assert.Equal(ForecastPersonId.From(1500000), first.Id);
        Assert.Equal("Testing", first.FirstName);
        Assert.Equal("Person", first.LastName);
        Assert.Equal("testing@example.com", first.Email);
        Assert.True(first.Admin);
        Assert.False(first.Archived);
        Assert.True(first.Subscribed);
        Assert.Equal(DateTime.Parse("2017-09-28T23:23:51.111Z"), first.UpdatedAt);
        Assert.Null(first.UpdatedById);
        Assert.Equal(
            new WorkingDays(new[] { DayOfWeek.Monday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday }),
            first.WorkingDays);
    }

    [Fact]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>(async () => await client.GetPeopleAsync());
    }
}
