namespace Cef.Core.Options
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class UserOptions
    {
        public User[] Users { get; set; }
    }

    [PublicAPI]
    public class User
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
