using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HarvestForecast.Client.Json;

/// <summary>
///     A converter that only writes the <see cref="DateTime.Date" /> part of a <see cref="DateTime" /> value.
/// </summary>
internal class LocalNullableDateOnlyConverter : JsonConverter<DateOnly?>
{
    public override DateOnly? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        return DateConversion.Read( ref reader );
    }

    public override void Write( Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options )
    {
        DateConversion.Write( writer, value );
    }
}
