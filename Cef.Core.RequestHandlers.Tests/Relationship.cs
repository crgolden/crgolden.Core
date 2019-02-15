namespace Cef.Core.RequestHandlers.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    internal class Relationship : RelationshipBase<Model, Model>
    {
        public string TestName { get; set; }

        internal Relationship(Guid model1Id, string model1Name, Guid model2Id, string model2Name)
            : base(model1Id, model1Name, model2Id, model2Name)
        {
        }
    }
}