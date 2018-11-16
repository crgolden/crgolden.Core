namespace Cef.Core.Options
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class UserOptions
    {
        public UserOption[] Users { get; set; }
    }

    [PublicAPI]
    public class UserOption
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
