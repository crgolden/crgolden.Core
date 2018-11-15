namespace Cef.Core.Options
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class CorsOptions
    {
        public string[] Origins { get; set; }
    }
}
