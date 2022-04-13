using System;
using System.Text.Json.Serialization;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     Information about a client who may have one or more <see cref="Project"/>s. 
/// </summary>
public record Client( [ property : JsonPropertyName( "id" ) ] int Id,
                      [ property : JsonPropertyName( "name" ) ] string Name,
                      [ property : JsonPropertyName( "harvest_id" ) ] int? HarvestId,
                      [ property : JsonPropertyName( "archived" ) ] bool Archived,
                      [ property : JsonPropertyName( "updated_at" ) ] DateTimeOffset UpdatedAt,
                      [ property : JsonPropertyName( "updated_by_id" ) ] int UpdatedById );
