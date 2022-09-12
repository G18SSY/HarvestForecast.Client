using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     A placeholder who is being scheduled.
/// </summary>
public record Placeholder( [ property : JsonPropertyName( "id" ) ]
                           int Id,
                           [ property : JsonPropertyName( "name" ) ]
                           string Name,
                           [ property : JsonPropertyName( "archived" ) ]
                           bool Archived,
                           [ property : JsonPropertyName( "roles" ) ]
                           IReadOnlyCollection<string> Roles,
                           [ property : JsonPropertyName( "updated_at" ) ]
                           DateTimeOffset UpdatedAt,
                           [ property : JsonPropertyName( "updated_by_id" ) ]
                           int? UpdatedById );
