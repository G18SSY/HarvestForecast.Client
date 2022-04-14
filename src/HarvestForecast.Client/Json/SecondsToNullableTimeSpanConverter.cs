using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HarvestForecast.Client.Json;

internal class SecondsToNullableTimeSpanConverter : JsonConverter<TimeSpan?>
{
    public override TimeSpan? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        return TimeConversion.Read( ref reader );
    }

    public override void Write( Utf8JsonWriter writer, TimeSpan? value, JsonSerializerOptions options )
    {
        TimeConversion.Write( writer, value );
    }
}
