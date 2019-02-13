namespace Cef.Core.Options
{
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class SqLiteOptions
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string SqLiteConnectionString => $"Data Source={Path}/{Name}.db";
    }
}
