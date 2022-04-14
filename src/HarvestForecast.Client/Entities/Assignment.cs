using System;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Json;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     An item of work assigned to a resource.
/// </summary>
/// <param name="Id">The unique ID number.</param>
/// <param name="Allocation">The time allocated to the task.</param>
/// <param name="StartDate">The first day of the assignment.</param>
/// <param name="EndDate">The last day of the assignment.</param>
/// <param name="Notes">Optional notes with more details.</param>
/// <param name="UpdatedAt">The timestamp of the last update.</param>
/// <param name="UpdatedById">The last user to update the <see cref="Assignment" />.</param>
/// <param name="ProjectId">The ID of the <see cref="Project" /> this assignment is for.</param>
/// <param name="PersonId">The ID of the <see cref="Person" /> this is assigned to.</param>
/// <param name="PlaceholderId">The ID of the <see cref="Placeholder" /> this is assigned to.</param>
/// <param name="RepeatedAssignmentSetId">The ID of the repeating assignment this is from.</param>
/// <param name="ActiveOnDaysOff">Indicates if the assignment continues during non-working days.</param>
public record Assignment( [ property : JsonPropertyName( "id" ) ] 
                          int Id,
                          [ property : JsonPropertyName( "start_date" ) ]
                          [ property : JsonConverter( typeof( LocalNullableDateOnlyConverter ) ) ]
                          DateTime? StartDate,
                          [ property : JsonPropertyName( "end_date" ) ]
                          [ property : JsonConverter( typeof( LocalNullableDateOnlyConverter ) ) ]
                          DateTime? EndDate,
                          [ property : JsonPropertyName( "allocation" ) ]
                          [ property : JsonConverter( typeof( SecondsToNullableTimeSpanConverter ) ) ]
                          TimeSpan? Allocation,
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