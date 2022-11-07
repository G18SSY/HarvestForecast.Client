using System;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Entities.VO;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     Information about a client who may have one or more <see cref="Project" />s.
/// </summary>
public record Client([property: JsonPropertyName("id")] ForecastClientId Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("harvest_id")]
    HarvestClientId? HarvestId,
    [property: JsonPropertyName("archived")]
    bool Archived,
    [property: JsonPropertyName("updated_at")]
    DateTimeOffset UpdatedAt,
    [property: JsonPropertyName("updated_by_id")]
    ForecastPersonId? UpdatedById);
