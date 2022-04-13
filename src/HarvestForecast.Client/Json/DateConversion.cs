using System;
using System.Globalization;
using System.Text.Json;

namespace HarvestForecast.Client.Json;

internal static class DateConversion
{
    public static DateTime? Read( ref Utf8JsonReader reader )
    {
        string? raw = reader.GetString();

        if ( string.IsNullOrEmpty( raw ) )
        {
            return null;
        }

        return DateTime.ParseExact( raw, DateUtility.DateOnlyFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal );
    }

    public static void Write( Utf8JsonWriter writer, DateTime? value )
    {
        if ( value.HasValue )
        {
            writer.WriteStringValue( DateUtility.FormatDateOnly( value.Value ) );
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
