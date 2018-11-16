namespace Cef.Core.Options
{
    using JetBrains.Annotations;

    public class SqLiteOptions
    {
        [PublicAPI]
        public string Name { get; set; }

        [PublicAPI]
        public string Path { get; set; }

        [PublicAPI]
        public string SqLiteConnectionString => $"Data Source={Path}/{Name}.db";
    }
}
