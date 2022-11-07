using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using HarvestForecast.Client.Entities;
using HarvestForecast.Client.Entities.VO;
using Xunit;

namespace HarvestForecast.Client.Test;

public class JsonTests
{
    [Fact]
    public async Task WritesValidNestedJsonBasedOnGenericType()
    {
        var assignment = new Assignment(ForecastAssignmentId.From(12234),
            null,
            new DateOnly(1066, 6, 2),
            TimeSpan.FromHours(2),
            "ABC",
            DateTimeOffset.UtcNow,
            null,
            ForecastProjectId.From(67890),
            ForecastPersonId.From(45789),
            null,
            null,
            true);

        const string propertyName = "assignment";
        var content = NestedJsonHelper.CreateNestedJsonContent<AssignmentData>(assignment, propertyName);

        await using var stream = await content.ReadAsStreamAsync();
        var doc = await JsonDocument.ParseAsync(stream);
        var root = doc.RootElement;
        root.ValueKind.Should().Be(JsonValueKind.Object);
        var rootChildren = root.EnumerateObject().ToList();
        var rootChild = rootChildren.Should().ContainSingle().Subject;
        rootChild.Name.Should().Be(propertyName);
        rootChild.Value.ValueKind.Should().Be(JsonValueKind.Object);
        var properties = rootChild.Value.EnumerateObject().ToList();

        ShouldContain("notes", e => e.GetString().Should().Be(assignment.Notes));
        ShouldContain("person_id", e => ForecastPersonId.From(e.GetInt32()).Should().Be(assignment.PersonId));
        ShouldContain("active_on_days_off", e => e.GetBoolean().Should().Be(assignment.ActiveOnDaysOff));
        ShouldNotContain("id");
        ShouldNotContain("updated_at");
        ShouldNotContain("updated_by_id");

        void ShouldContain(string property, Action<JsonElement> elementAssertion)
        {
            var prop = properties.Should()
                .ContainSingle(p => string.Equals(p.Name, property, StringComparison.OrdinalIgnoreCase)).Subject;
            elementAssertion(prop.Value);
        }

        void ShouldNotContain(string property)
        {
            properties.Should().NotContain(p => string.Equals(p.Name, property, StringComparison.OrdinalIgnoreCase));
        }
    }
}