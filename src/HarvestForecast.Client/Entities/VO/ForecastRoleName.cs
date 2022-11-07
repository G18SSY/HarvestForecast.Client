using Vogen;

namespace HarvestForecast.Client.Entities.VO;

/// <summary>
///     The <see cref="Role.Name" /> of a <see cref="Role" /> within Forecast.
/// </summary>
[ValueObject(typeof(string))]
public readonly partial struct ForecastRoleName
{
}