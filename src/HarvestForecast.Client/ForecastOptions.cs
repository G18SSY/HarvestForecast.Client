namespace HarvestForecast.Client;

/// <summary>
///     Options for controlling the <see cref="ForecastClient" />.
/// </summary>
public class ForecastOptions
{
    /// <summary>
    ///     The ID of the account to access.
    /// </summary>
    /// <remarks>Find this in the URL you use for Forecast (https://forecastapp.com/ACCOUNT_ID/schedule/projects).</remarks>
    public string AccountId { get; }

    /// <summary>
    ///     A Harvest ID personal access token.
    /// </summary>
    /// <remarks>Obtain from https://id.getharvest.com/oauth2/access_tokens/new</remarks>
    public string AccessToken { get; }

    /// <summary>
    ///     Creates a new set of options.
    /// </summary>
    /// <param name="accountId">The ID of the account to access.</param>
    /// <param name="accessToken">A Harvest ID personal access token.</param>
    public ForecastOptions( string accountId, string accessToken )
    {
        AccountId = accountId;
        AccessToken = accessToken;
    }
}
