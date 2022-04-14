using System.Threading.Tasks;

namespace HarvestForecast.Client;

internal static class HttpExtensions
{
    /// <summary>
    ///     Suppresses a response with a code of 404 and returns null instead.
    /// </summary>
    public static async ValueTask<T?> ReturnNullIfNotFoundAsync<T>( this ValueTask<T> task )
        where T : class
    {
        try
        {
            return await task;
        }
        catch ( NotFoundException )
        {
            return null;
        }
    }
}
