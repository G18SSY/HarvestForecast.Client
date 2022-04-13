using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HarvestForecast.Client.Json;

/// <summary>
///     A converter that only writes the <see cref="DateTime.Date" /> part of a <see cref="DateTime" /> value.
/// </summary>
internal class LocalDateOnlyConverter : JsonConverter<DateTime?>
{
    public override DateTime? Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        Debug.Assert( typeToConvert == typeof( DateTime ) || typeToConvert == typeof( DateTime? ) );

        string? raw = reader.GetString();

        if ( string.IsNullOrEmpty( raw ) )
        {
            return null;
        }

        return DateTime.ParseExact( raw, DateUtility.DateOnlyFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal );
    }

    public override void Write( Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options )
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
