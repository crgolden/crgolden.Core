namespace Clarity.Core
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class AddressOptions
    {
        public SmartyStreetsOptions SmartyStreetsOptions { get; set; }
    }

    [PublicAPI]
    public class SmartyStreetsOptions
    {
        public string AuthId { get; set; }

        public string AuthToken { get; set; }
    }
}