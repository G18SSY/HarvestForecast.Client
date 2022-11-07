using System.Collections.Generic;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Entities.VO;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     Contains information about the current Forecast user.
/// </summary>
/// <param name="Id">The ID of the user.</param>
/// <param name="AccountIds">The IDs of the accounts that this user is part of.</param>
public record CurrentUser([property: JsonPropertyName("id")] ForecastPersonId Id,
    [property: JsonPropertyName("account_ids")]
    IReadOnlyCollection<ForecastAccountId> AccountIds);