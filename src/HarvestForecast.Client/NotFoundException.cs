using System;
using System.Net;
using System.Runtime.Serialization;

namespace HarvestForecast.Client;

/// <summary>
///     An exception that is thrown when a request returns a <see cref="HttpStatusCode.NotFound" /> status.
/// </summary>
[ Serializable ]
public class NotFoundException : Exception
{
    /// <inheritdoc />
    public NotFoundException() { }

    /// <inheritdoc />
    public NotFoundException( string message ) : base( message ) { }

    /// <inheritdoc />
    public NotFoundException( string message, Exception inner ) : base( message, inner ) { }

    /// <inheritdoc />
    protected NotFoundException(
        SerializationInfo info,
        StreamingContext context ) : base( info, context ) { }
}
