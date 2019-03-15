namespace Clarity.Core
{
    using Newtonsoft.Json;

    /// <include file='docs.xml' path='docs/members[@name="address"]/Address/*'/>
    public class Address
    {
        /// <include file='docs.xml' path='docs/members[@name="address"]/Formatted/*'/>
        [JsonProperty("formatted")]
        public string Formatted => $"{StreetAddress}\r\n{Locality}\r\n{Region} {PostalCode}\r\n{Country}";

        /// <include file='docs.xml' path='docs/members[@name="address"]/StreetAddress/*'/>
        [JsonProperty("street_address")]
        public string StreetAddress { get; set; }

        /// <include file='docs.xml' path='docs/members[@name="address"]/Locality/*'/>
        [JsonProperty("locality")]
        public string Locality { get; set; }

        /// <include file='docs.xml' path='docs/members[@name="address"]/Region/*'/>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <include file='docs.xml' path='docs/members[@name="address"]/PostalCode/*'/>
        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }

        /// <include file='docs.xml' path='docs/members[@name="address"]/Country/*'/>
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
