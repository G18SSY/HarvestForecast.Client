using System;
using System.Globalization;

namespace HarvestForecast.Client;

internal static class DateUtility
{
    public const string DateOnlyFormat = "yyyy-MM-dd";

    public static string FormatDateOnly( DateTime value )
    {
        return value.ToString( DateOnlyFormat, CultureInfo.InvariantCulture );
    }
}
