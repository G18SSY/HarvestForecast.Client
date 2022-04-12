using System.Threading.Tasks;

namespace HarvestForecast.Client;

/// <summary>
///     A client which provides strongly typed access to the Harvest Forecast (unofficial) API.
/// </summary>
public interface IForecastClient
{
    /// <summary>
    ///     Gets the <see cref="CurrentUser" /> object for the logged in user.
    /// </summary>
    ValueTask<CurrentUser> WhoAmIAsync();
}