using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Entities;

namespace HarvestForecast.Client.Json;

internal class WorkingDaysConverter : JsonConverter<WorkingDays>
{
    public override WorkingDays Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
    {
        ReadRequiredToken( ref reader, JsonTokenType.StartObject );

        WorkingDays days = new();
        while ( reader.TokenType == JsonTokenType.PropertyName )
        {
            ( var day, bool active ) = ReadDay( ref reader );

            if ( active )
            {
                days += day;
            }
        }

        AssertRequiredToken( ref reader, JsonTokenType.EndObject );

        return days;
    }

    private (DayOfWeek, bool) ReadDay( ref Utf8JsonReader reader )
    {
        AssertRequiredToken( ref reader, JsonTokenType.PropertyName );

        string dayRaw = reader.GetString() ?? throw new JsonException( "Property name cannot be null" );
        ReadAndRequire( ref reader );
        var day = (DayOfWeek) Enum.Parse( typeof( DayOfWeek ), dayRaw, true );

        bool active = reader.GetBoolean();
        ReadAndRequire( ref reader );

        return ( day, active );
    }

    private static void AssertRequiredToken( ref Utf8JsonReader reader, JsonTokenType type )
    {
        if ( reader.TokenType != type )
        {
            throw new JsonException( $"Expected a {type}" );
        }
    }

    private static void ReadRequiredToken( ref Utf8JsonReader reader, JsonTokenType type )
    {
        AssertRequiredToken( ref reader, type );
        ReadAndRequire( ref reader );
    }

    private static void ReadAndRequire( ref Utf8JsonReader reader )
    {
        if ( !reader.Read() )
        {
            throw new JsonException( "Encountered end of content unexpectedly" );
        }
    }

    public override void Write( Utf8JsonWriter writer, WorkingDays value, JsonSerializerOptions options )
    {
        writer.WriteStartObject();
        writer.WriteBoolean( "monday", value.Monday );
        writer.WriteBoolean( "tuesday", value.Tuesday );
        writer.WriteBoolean( "wednesday", value.Wednesday );
        writer.WriteBoolean( "thursday", value.Thursday );
        writer.WriteBoolean( "friday", value.Friday );
        writer.WriteEndObject();
    }
}
