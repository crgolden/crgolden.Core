namespace Cef.Core.Options
{
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class PaymentOptions
    {
        public Stripe Stripe { get; set; }
    }

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class Stripe
    {
        public string SecretKey { get; set; }
    }
}