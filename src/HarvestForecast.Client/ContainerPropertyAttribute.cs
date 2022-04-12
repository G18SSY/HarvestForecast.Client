using System;

namespace HarvestForecast.Client;

[ AttributeUsage( AttributeTargets.Class, Inherited = false ) ]
internal sealed class ContainerPropertyAttribute : Attribute
{
    /// <summary>
    ///     The name of the property that contains the value in a JSON response.
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Creates a new instance of the <see cref="ContainerPropertyAttribute" /> class.
    /// </summary>
    /// <param name="name">The name of the property that contains the value in a JSON response.</param>
    public ContainerPropertyAttribute( string name )
    {
        Name = name;
    }
}
