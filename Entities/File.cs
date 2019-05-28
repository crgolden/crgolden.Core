namespace Clarity.Core
{
    using System.Diagnostics.CodeAnalysis;
    using Abstractions;

    [ExcludeFromCodeCoverage]
    public abstract class File : Entity
    {
        public string ContentType { get; set; }

        public string Extension { get; set; }

        public string Name { get; set; }

        public long Size { get; set; }

        public string Uri { get; set; }
    }
}