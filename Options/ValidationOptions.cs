namespace Clarity.Core
{
    public class ValidationOptions
    {
        public SmartyStreetsOptions SmartyStreetsOptions { get; set; }
    }

    public class SmartyStreetsOptions
    {
        public string AuthId { get; set; }

        public string AuthToken { get; set; }
    }
}