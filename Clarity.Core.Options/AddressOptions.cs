namespace Clarity.Core
{
    public class AddressOptions
    {
        public SmartyStreetsOptions SmartyStreetsOptions { get; set; }
    }

    public class SmartyStreetsOptions
    {
        public string AuthId { get; set; }

        public string AuthToken { get; set; }
    }
}