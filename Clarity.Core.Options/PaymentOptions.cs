namespace Clarity.Core
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class PaymentOptions
    {
        public StripeOptions StripeOptions { get; set; }
    }

    [PublicAPI]
    public class StripeOptions
    {
        public string SecretKey { get; set; }
    }
}