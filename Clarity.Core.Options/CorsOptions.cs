namespace Clarity.Core
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class CorsOptions
    {
        public string[] Origins { get; set; }
    }
}