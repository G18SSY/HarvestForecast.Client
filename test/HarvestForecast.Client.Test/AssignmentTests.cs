using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HarvestForecast.Client.Entities;
using Xunit;

namespace HarvestForecast.Client.Test;

public class AssignmentTests
{
    [ Fact ]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var assignments = await client.GetAssignmentsAsync( AssignmentFilter.Today() );

        Assert.NotNull( assignments );
        Assert.NotEmpty( assignments );

        var first = assignments.First();
        Assert.Equal( 1000001, first.Id );
        Assert.Equal( new DateTime( 2017, 05, 24 ), first.StartDate );
        Assert.Equal( new DateTime( 2017, 05, 29 ), first.EndDate );
        Assert.Null( first.Allocation );
        Assert.Null( first.Notes );
        Assert.Equal( DateTime.Parse( "2017-05-02T19:07:00.478Z" ), first.UpdatedAt );
        Assert.Equal( 111111, first.UpdatedById );
        Assert.Equal( 222222, first.ProjectId );
        Assert.Equal( 333333, first.PersonId );
        Assert.Null( first.PlaceholderId );
        Assert.Null( first.RepeatedAssignmentSetId );
    }

    [ Fact ]
    public void CanFilterByDates()
    {
        var today = DateTime.Today;

        var filter = new AssignmentFilter( today, today );
        var filters = filter.GetActiveFilters().ToList();

        string todayFormatted = DateUtility.FormatDateOnly(today);
        Assert.Equal(2, filters.Count);
        Assert.Contains(new KeyValuePair<string, string>("start_date", todayFormatted), filters);
        Assert.Contains(new KeyValuePair<string, string>("end_date", todayFormatted), filters);
    }

    [ Fact ]
    public void CanFilterByProjectId()
    {
        var today = DateTime.Today;
        const int projectId = 567823;
        
        var filter = new AssignmentFilter( today, today )
        {
            ProjectId = projectId
        };
        var filters = filter.GetActiveFilters().ToList();

        Assert.Equal(3, filters.Count);
        Assert.Contains(new KeyValuePair<string, string>("project_id", projectId.ToString()), filters);
    }

    [ Fact ]
    public void CanFilterByPersonId()
    {
        var today = DateTime.Today;
        const int personId = 564523;
        
        var filter = new AssignmentFilter( today, today )
        {
            PersonId = personId
        };
        var filters = filter.GetActiveFilters().ToList();

        Assert.Equal(3, filters.Count);
        Assert.Contains(new KeyValuePair<string, string>("person_id", personId.ToString()), filters);
    }

    [ Fact ]
    public void CanFilterByRepeatedAssignmentSetId()
    {
        var today = DateTime.Today;
        const int repeatedAssignmentSetId = 23;
        
        var filter = new AssignmentFilter( today, today )
        {
            RepeatedAssignmentSetId = repeatedAssignmentSetId
        };
        var filters = filter.GetActiveFilters().ToList();

        Assert.Equal(3, filters.Count);
        Assert.Contains(new KeyValuePair<string, string>("repeated_assignment_set_id", repeatedAssignmentSetId.ToString()), filters);
    }

    [ Fact ]
    public void CanFilterByState()
    {
        var today = DateTime.Today;
        
        var filter = new AssignmentFilter( today, today )
        {
            State = AssigmentState.Archived
        };
        var filters = filter.GetActiveFilters().ToList();

        Assert.Equal(3, filters.Count);
        Assert.Contains(new KeyValuePair<string, string>("state", "archived"), filters);
    }

    [ Fact ]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>( async () => await client.GetAssignmentsAsync( AssignmentFilter.Today() ) );
    }
}
