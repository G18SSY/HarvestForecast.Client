using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HarvestForecast.Client;

/// <summary>
///     Contains information about the current Forecast user.
/// </summary>
/// <param name="Id">The ID of the user.</param>
/// <param name="AccountIds">The IDs of the accounts that this user is part of.</param>
public record CurrentUser( [ property : JsonPropertyName( "id" ) ] int Id,
                           [ property : JsonPropertyName( "account_ids" ) ] IReadOnlyCollection<int> AccountIds );

internal record CurrentUserContainer( [ property : JsonPropertyName( "current_user" ) ] CurrentUser CurrentUser );
