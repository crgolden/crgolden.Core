namespace Cef.Core.Options
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class SqLiteOptions
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string SqLiteConnectionString => $"Data Source={Path}/{Name}.db";
    }
}
