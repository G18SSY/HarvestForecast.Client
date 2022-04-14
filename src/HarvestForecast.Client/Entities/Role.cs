using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     Information about set of skills which can be used to group people and placeholders. 
/// </summary>
public record Role( [ property : JsonPropertyName( "id" ) ] int Id,
                      [ property : JsonPropertyName( "name" ) ] string Name,
                      [ property : JsonPropertyName( "harvest_role_id" ) ] int? HarvestRoleId,
                      [ property : JsonPropertyName( "placeholder_ids" ) ] IReadOnlyCollection<int> PlaceholderIds ,
                      [ property : JsonPropertyName( "person_ids" ) ] IReadOnlyCollection<int> PersonIds );
