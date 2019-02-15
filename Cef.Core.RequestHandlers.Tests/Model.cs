namespace Cef.Core.RequestHandlers.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    internal class Model : Core.ModelBase
    {
        internal string TestName { get; set; }

        internal Model(string name) : base(name)
        {
        }

        internal Model(string name, Guid id) : base(name, id)
        {
        }
    }
}