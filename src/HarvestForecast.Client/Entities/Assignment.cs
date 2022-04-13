using System;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Json;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     An item of work assigned to a resource.
/// </summary>
public record Assignment( [ property : JsonPropertyName( "id" ) ] 
                          int Id,
                          [ property : JsonPropertyName( "start_date" ) ]
                          [ property : JsonConverter( typeof( LocalDateOnlyConverter ) ) ]
                          DateTime? StartDate,
                          [ property : JsonPropertyName( "end_date" ) ]
                          [ property : JsonConverter( typeof( LocalDateOnlyConverter ) ) ]
                          DateTime? EndDate,
                          [ property : JsonPropertyName( "allocation" ) ]
                          int? Allocation,
                          [ property : JsonPropertyName( "notes" ) ]
                          string? Notes ,
                          [ property : JsonPropertyName( "updated_at" ) ]
                          DateTimeOffset UpdatedAt ,
                          [ property : JsonPropertyName( "updated_by_id" ) ]
                          int UpdatedById ,
                          [ property : JsonPropertyName( "project_id" ) ]
                          int ProjectId ,
                          [ property : JsonPropertyName( "person_id" ) ]
                          int? PersonId ,
                          [ property : JsonPropertyName( "placeholder_id" ) ]
                          int? PlaceholderId ,
                          [ property : JsonPropertyName( "repeated_assignment_set_id" ) ]
                          int? RepeatedAssignmentSetId,
                          [ property : JsonPropertyName( "active_on_days_off" ) ]
                          bool ActiveOnDaysOff );