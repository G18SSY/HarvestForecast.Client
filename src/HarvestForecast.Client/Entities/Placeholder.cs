using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Entities.VO;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     A placeholder who is being scheduled.
/// </summary>
public record Placeholder([property: JsonPropertyName("id")] ForecastPlaceholderId Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("archived")]
    bool Archived,
    [property: JsonPropertyName("roles")] IReadOnlyCollection<ForecastRoleName> Roles,
    [property: JsonPropertyName("updated_at")]
    DateTimeOffset UpdatedAt,
    [property: JsonPropertyName("updated_by_id")]
    ForecastPersonId? UpdatedById);
