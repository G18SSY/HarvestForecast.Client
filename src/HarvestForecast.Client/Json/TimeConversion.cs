using System;
using System.Text.Json;

namespace HarvestForecast.Client.Json;

internal static class TimeConversion
{
    public static TimeSpan? Read( ref Utf8JsonReader reader )
    {
        if ( reader.TokenType == JsonTokenType.Null )
        {
            return null;
        }

        int seconds = reader.GetInt32();
        return TimeSpan.FromSeconds( seconds );
    }

    public static void Write( Utf8JsonWriter writer, TimeSpan? value )
    {
        if ( value.HasValue )
        {
            writer.WriteNumberValue( value.Value.TotalSeconds );
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}
