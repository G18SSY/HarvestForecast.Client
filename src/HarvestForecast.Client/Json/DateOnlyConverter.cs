using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HarvestForecast.Client.Json;

/// <summary>
///     A converter that only writes the <see cref="DateTime.Date" /> part of a <see cref="DateTime" /> value.
/// </summary>
internal class LocalDateOnlyConverter : JsonConverter<DateTime>
{
    public override DateTime Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        return DateConversion.Read( ref reader )!.Value;
    }

    public override void Write( Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options )
    {
        DateConversion.Write( writer, value );
    }
}
