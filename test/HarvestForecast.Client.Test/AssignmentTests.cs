using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using HarvestForecast.Client.Entities;
using RichardSzalay.MockHttp;
using Xunit;

namespace HarvestForecast.Client.Test;

public class AssignmentTests
{
    [Fact]
    public async Task CanDeserializeResponse()
    {
        var client = ApiTestHelper.GetMockedForecastClient();

        var assignments = await client.GetAssignmentsAsync(AssignmentFilter.Today());

        Assert.NotNull(assignments);
        Assert.NotEmpty(assignments);

        var first = assignments.First();
        Assert.Equal(1000001, first.Id);
        Assert.Equal(new DateOnly(2017, 05, 24), first.StartDate);
        Assert.Equal(new DateOnly(2017, 05, 29), first.EndDate);
        Assert.Null(first.Allocation);
        Assert.Null(first.Notes);
        Assert.Equal(DateTime.Parse("2017-05-02T19:07:00.478Z"), first.UpdatedAt);
        Assert.Equal(111111, first.UpdatedById);
        Assert.Equal(222222, first.ProjectId);
        Assert.Equal(333333, first.PersonId);
        Assert.Null(first.PlaceholderId);
        Assert.Null(first.RepeatedAssignmentSetId);
    }

    [Fact]
    public void CanFilterByDates()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var filter = new AssignmentFilter(today, today);
        var filters = filter.GetActiveFilters().ToList();

        string todayFormatted = DateUtility.FormatDateOnly(today);
        Assert.Equal(2, filters.Count);
        Assert.Contains(new KeyValuePair<string, string>("start_date", todayFormatted), filters);
        Assert.Contains(new KeyValuePair<string, string>("end_date", todayFormatted), filters);
    }

    [Fact]
    public void CanFilterByProjectId()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        const int projectId = 567823;

        var filter = new AssignmentFilter(today, today)
        {
            ProjectId = projectId
        };
        var filters = filter.GetActiveFilters().ToList();

        Assert.Equal(3, filters.Count);
        Assert.Contains(new KeyValuePair<string, string>("project_id", projectId.ToString()), filters);
    }

    [Fact]
    public void CanFilterByPersonId()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        const int personId = 564523;

        var filter = new AssignmentFilter(today, today)
        {
            PersonId = personId
        };
        var filters = filter.GetActiveFilters().ToList();

        Assert.Equal(3, filters.Count);
        Assert.Contains(new KeyValuePair<string, string>("person_id", personId.ToString()), filters);
    }

    [Fact]
    public void CanFilterByRepeatedAssignmentSetId()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        const int repeatedAssignmentSetId = 23;

        var filter = new AssignmentFilter(today, today)
        {
            RepeatedAssignmentSetId = repeatedAssignmentSetId
        };
        var filters = filter.GetActiveFilters().ToList();

        Assert.Equal(3, filters.Count);
        Assert.Contains(
            new KeyValuePair<string, string>("repeated_assignment_set_id", repeatedAssignmentSetId.ToString()),
            filters);
    }

    [Fact]
    public void CanFilterByState()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        var filter = new AssignmentFilter(today, today)
        {
            State = AssigmentState.Archived
        };
        var filters = filter.GetActiveFilters().ToList();

        Assert.Equal(3, filters.Count);
        Assert.Contains(new KeyValuePair<string, string>("state", "archived"), filters);
    }

    [Fact]
    public async Task ThrowsOnFailedResponse()
    {
        var client = ApiTestHelper.GetFailedRequestForecastClient();

        await Assert.ThrowsAsync<HttpRequestException>(async () =>
            await client.GetAssignmentsAsync(AssignmentFilter.Today()));
    }

    [Fact]
    public async Task CanCreate()
    {
        var handler = new MockHttpMessageHandler();
        handler.Expect(HttpMethod.Post, ApiTestHelper.GetFullPath("assignments"))
            .RespondWithJsonTestData("assignment.json");
        var httpClient = handler.ToHttpClient();
        IForecastClient client = new ForecastClient(httpClient, ApiTestHelper.TestOptions);

        var data = MockAssignmentData();
        var created = await client.CreateAssignmentAsync(data);

        handler.VerifyNoOutstandingExpectation();
        created.Id.Should().Be(1000001);
    }

    [Fact]
    public async Task CanUpdate()
    {
        const int id = 1000001;
        var handler = new MockHttpMessageHandler();
        handler.Expect(HttpMethod.Put, ApiTestHelper.GetFullPath($"assignments/{id}"))
            .RespondWithJsonTestData("assignment.json");
        var httpClient = handler.ToHttpClient();
        IForecastClient client = new ForecastClient(httpClient, ApiTestHelper.TestOptions);

        var data = MockAssignmentData();
        var created = await client.UpdateAssignmentAsync(id, data);

        handler.VerifyNoOutstandingExpectation();
        created.Should().NotBeNull();
    }

    [Fact]
    public async Task CanRemove()
    {
        const int id = 1000001;
        var handler = new MockHttpMessageHandler();
        handler.Expect(HttpMethod.Delete, ApiTestHelper.GetFullPath($"assignments/{id}"))
            .Respond(HttpStatusCode.NoContent);
        handler.Expect(HttpMethod.Delete, ApiTestHelper.GetFullPath($"assignments/{id}"))
            .Respond(HttpStatusCode.NotFound);
        var httpClient = handler.ToHttpClient();
        IForecastClient client = new ForecastClient(httpClient, ApiTestHelper.TestOptions);

        var deleted1 = await client.RemoveAssignmentAsync(id);
        var deleted2 = await client.RemoveAssignmentAsync(id);

        handler.VerifyNoOutstandingExpectation();
        deleted1.Should().BeTrue();
        deleted2.Should().BeFalse();
    }

    private static AssignmentData MockAssignmentData()
        => new(DateOnly.Parse("2017-05-24"),
            DateOnly.Parse("2017-05-29"),
            null,
            null,
            222222,
            333333,
            null,
            null,
            true);
}