using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Entities.VO;
using HarvestForecast.Client.Json;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     An project is a group of work items for a specific <see cref="Client" />.
/// </summary>
public record Project([property: JsonPropertyName("id")] ForecastProjectId Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("color")] string Color,
    [property: JsonPropertyName("code")] string Code,
    [property: JsonPropertyName("start_date")]
    [property: JsonConverter(typeof(LocalNullableDateOnlyConverter))]
    DateOnly? StartDate,
    [property: JsonPropertyName("end_date")]
    [property: JsonConverter(typeof(LocalNullableDateOnlyConverter))]
    DateOnly? EndDate,
    [property: JsonPropertyName("notes")] string? Notes,
    [property: JsonPropertyName("harvest_id")]
    HarvestProjectId? HarvestId,
    [property: JsonPropertyName("archived")]
    bool Archived,
    [property: JsonPropertyName("updated_at")]
    DateTimeOffset UpdatedAt,
    [property: JsonPropertyName("updated_by_id")]
    ForecastPersonId? UpdatedById,
    [property: JsonPropertyName("client_id")]
    ForecastClientId? ClientId,
    [property: JsonPropertyName("tags")] IReadOnlyCollection<string> Tags);
