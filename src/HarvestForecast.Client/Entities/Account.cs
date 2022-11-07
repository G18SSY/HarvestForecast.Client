using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Entities.VO;
using HarvestForecast.Client.Json;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     An account configuration in Forecast.
/// </summary>
public record Account([property: JsonPropertyName("id")] ForecastAccountId Id,
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("weekly_capacity")]
    [property: JsonConverter(typeof(SecondsToTimeSpanConverter))]
    TimeSpan WeeklyCapacity,
    [property: JsonPropertyName("color_labels")]
    IReadOnlyCollection<ColorLabel> ColorLabels,
    [property: JsonPropertyName("harvest_subdomain")]
    string HarvestSubDomain,
    [property: JsonPropertyName("harvest_name")]
    string HarvestName);