using System.Text.Json.Serialization;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     Configuration for assignment coloring in an <see cref="Account" />.
/// </summary>
/// <param name="Name">The name of the color, e.g. aqua</param>
/// <param name="Label">The label to show, e.g. Internal</param>
public record ColorLabel( [ property : JsonPropertyName( "name" ) ] string Name,
                          [ property : JsonPropertyName( "label" ) ] string Label );
