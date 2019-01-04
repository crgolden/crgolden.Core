namespace Cef.Core.Options
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class EmailOptions
    {
        public string ApiKey { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
    }
}