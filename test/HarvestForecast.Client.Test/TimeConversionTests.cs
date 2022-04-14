using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using HarvestForecast.Client.Json;
using Xunit;

namespace HarvestForecast.Client.Test;

public class TimeConversionTests
{
    public static IEnumerable<object[]> SecondsJsonWithTimespans
    {
        get
        {
            yield return new object[] {ConstructMyTimeJson( "0" ), TimeSpan.Zero};
            yield return new object[] {ConstructMyTimeJson( "25200" ), TimeSpan.FromHours( 7 )};
            yield return new object[] {ConstructMyTimeJson( "12600" ), TimeSpan.FromHours( 3.5 )};
        }
    }

    [ MemberData( nameof( SecondsJsonWithTimespans ) ) ]
    [ Theory ]
    public void CanConvertFromSecondsWithNonNullable( string json, TimeSpan expected )
    {
        var value = JsonSerializer.Deserialize<MyTime>( json );

        Assert.NotNull( value );
        Assert.Equal( expected, value!.Time );
    }

    [ MemberData( nameof( SecondsJsonWithTimespans ) ) ]
    [ Theory ]
    public void CanConvertFromSecondsWithNullable( string json, TimeSpan expected )
    {
        var value = JsonSerializer.Deserialize<MyNullableTime>( json );

        Assert.NotNull( value );
        Assert.Equal( expected, value!.Time );
    }

    [ MemberData( nameof( SecondsJsonWithTimespans ) ) ]
    [ Theory ]
    public void CanConvertToSecondsWithNonNullable( string expected, TimeSpan timeSpan )
    {
        string value = JsonSerializer.Serialize( new MyTime( timeSpan ) );

        Assert.Equal( expected, value );
    }

    [ MemberData( nameof( SecondsJsonWithTimespans ) ) ]
    [ Theory ]
    public void CanConvertToSecondsWithNullable( string expected, TimeSpan timeSpan )
    {
        string value = JsonSerializer.Serialize( new MyNullableTime( timeSpan ) );

        Assert.Equal( expected, value );
    }

    private static string ConstructMyTimeJson( string time )
    {
        return $"{{\"Time\":{time}}}";
    }

    private record MyTime( [ property : JsonConverter( typeof( SecondsToTimeSpanConverter ) ) ] TimeSpan Time );

    private record MyNullableTime( [ property : JsonConverter( typeof( SecondsToNullableTimeSpanConverter ) ) ] TimeSpan? Time );


    [ Fact ]
    public void CanConvertFromNull()
    {
        string json = ConstructMyTimeJson( "null" );

        var value = JsonSerializer.Deserialize<MyNullableTime>( json );

        Assert.NotNull( value );
        Assert.Null( value!.Time );
    }

    [ Fact ]
    public void CanConvertToNull()
    {
        string json = JsonSerializer.Serialize( new MyNullableTime( null ) );

        string expected = ConstructMyTimeJson( "null" );
        Assert.Equal( expected, json );
    }
}
