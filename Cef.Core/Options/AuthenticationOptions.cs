namespace Cef.Core.Options
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class AuthenticationOptions
    {
        public Facebook Facebook { get; set; }
    }

    [PublicAPI]
    public class Facebook
    {
        public string AppId { get; set; }

        public string AppSecret { get; set; }
    }
}