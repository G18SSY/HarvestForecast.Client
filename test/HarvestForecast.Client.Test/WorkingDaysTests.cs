using System;
using System.Linq;
using System.Text.Json;
using HarvestForecast.Client.Entities;
using Xunit;

namespace HarvestForecast.Client.Test;

public class WorkingDaysTests
{
    [ Fact ]
    public void CanActivateDayWithOperator()
    {
        var original = new WorkingDays( new[] {DayOfWeek.Monday, DayOfWeek.Friday} );

        Assert.True( original.Monday );
        Assert.True( original.Friday );
        Assert.False( original.Wednesday );

        var transformed = original + DayOfWeek.Wednesday;

        Assert.True( transformed.Monday );
        Assert.True( transformed.Friday );
        Assert.True( transformed.Wednesday );
    }

    [ Fact ]
    public void CanDeactivateDayWithOperator()
    {
        var original = new WorkingDays( new[] {DayOfWeek.Monday, DayOfWeek.Friday} );

        Assert.True( original.Monday );
        Assert.True( original.Friday );

        var transformed = original - DayOfWeek.Monday;

        Assert.False( transformed.Monday );
        Assert.True( transformed.Friday );
    }

    [ Fact ]
    public void CanDeserializeJson()
    {
        const string json = @"{
				""monday"": true,
				""tuesday"": true,
				""wednesday"": false,
				""thursday"": true,
				""friday"": false
			}";

        var days = JsonSerializer.Deserialize<WorkingDays>( json );

        Assert.True( days.Monday );
        Assert.True( days.Tuesday );
        Assert.False( days.Wednesday );
        Assert.True( days.Thursday );
        Assert.False( days.Friday );
    }

    [ Fact ]
    public void CanGetDays()
    {
        var original = new WorkingDays( new[] {DayOfWeek.Monday, DayOfWeek.Friday} );

        var days = original.GetDays().ToList();
        Assert.Equal( 2, days.Count );
        Assert.Contains( DayOfWeek.Friday, days );
        Assert.Contains( DayOfWeek.Monday, days );
    }

    [ Fact ]
    public void CanRoundTripJson()
    {
        var original = new WorkingDays( new[] {DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Thursday} );

        string json = JsonSerializer.Serialize( original );
        var second = JsonSerializer.Deserialize<WorkingDays>( json );

        Assert.Equal( original, second );
    }
}
