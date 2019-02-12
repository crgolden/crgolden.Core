namespace Cef.Core.Options
{
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class AddressOptions
    {
        public SmartyStreets SmartyStreets { get; set; }
    }

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class SmartyStreets
    {
        public string AuthId { get; set; }

        public string AuthToken { get; set; }
    }
}