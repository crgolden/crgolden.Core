namespace Cef.Core.RequestHandlers.Tests
{
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    internal class Relationship : BaseRelationship<Model, Model>
    {
        public string Name { get; set; }
    }
}