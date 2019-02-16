namespace Clarity.Core.Fakes
{
    using System;

    internal class FakeEntity : Entity
    {
        internal Guid Id { get; private set; }

        internal string Description { get; set; }

        internal FakeEntity(string name) : base(name)
        {
        }

        internal FakeEntity(string name, Guid id) : this(name)
        {
            Id = id;
        }
    }
}