using System.Collections.Generic;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Entities.VO;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     Information about set of skills which can be used to group people and placeholders.
/// </summary>
public record Role([property: JsonPropertyName("id")] ForecastRoleId Id,
    [property: JsonPropertyName("name")] ForecastRoleName Name,
    [property: JsonPropertyName("harvest_role_id")]
    HarvestRoleId? HarvestRoleId,
    [property: JsonPropertyName("placeholder_ids")]
    IReadOnlyCollection<ForecastPlaceholderId> PlaceholderIds,
    [property: JsonPropertyName("person_ids")]
    IReadOnlyCollection<ForecastPersonId> PersonIds);
