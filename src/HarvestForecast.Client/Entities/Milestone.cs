using System;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Entities.VO;
using HarvestForecast.Client.Json;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     An dated project event.
/// </summary>
public record Milestone([property: JsonPropertyName("id")] ForecastMilestoneId Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("date")]
    [property: JsonConverter(typeof(LocalDateOnlyConverter))]
    DateOnly Date,
    [property: JsonPropertyName("updated_at")]
    DateTimeOffset UpdatedAt,
    [property: JsonPropertyName("updated_by_id")]
    ForecastPersonId? UpdatedById,
    [property: JsonPropertyName("project_id")]
    ForecastProjectId ProjectId);
