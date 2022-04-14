using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Json;

namespace HarvestForecast.Client.Entities;

/// <summary>
///     Information about a person with a profile configured. 
/// </summary>
public record Person( [ property : JsonPropertyName( "id" ) ] int Id,
                      [ property : JsonPropertyName( "first_name" ) ] string FirstName,
                      [ property : JsonPropertyName( "last_name" ) ] string LastName,
                      [ property : JsonPropertyName( "email" ) ] string Email,
                      [ property : JsonPropertyName( "login" ) ] string Login,
                      [ property : JsonPropertyName( "admin" ) ] bool Admin,
                      [ property : JsonPropertyName( "archived" ) ] bool Archived,
                      [ property : JsonPropertyName( "subscribed" ) ] bool Subscribed,
                      [ property : JsonPropertyName( "avatar_url" ) ] string AvatarUrl,
                      [ property : JsonPropertyName( "harvest_user_id" ) ] int? HarvestUserId,
                      [ property : JsonPropertyName( "weekly_capacity" ) ] 
                      [ property : JsonConverter( typeof( SecondsToTimeSpanConverter ) ) ]
                      TimeSpan WeeklyCapacity,
                      [ property : JsonPropertyName( "updated_at" ) ]
                      DateTimeOffset UpdatedAt,
                      [ property : JsonPropertyName( "updated_by_id" ) ] int UpdatedById,
                      [ property : JsonPropertyName( "color_blind" ) ] bool ColorBlind ,
                      [ property : JsonPropertyName( "roles" ) ] IReadOnlyCollection<string> Roles,
                      [ property : JsonPropertyName( "working_days" ) ] WorkingDays WorkingDays );