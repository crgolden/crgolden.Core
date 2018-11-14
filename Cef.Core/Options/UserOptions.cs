namespace Cef.Core.Options
{
    public class UserOptions
    {
        public User[] Users { get; set; }
    }

    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
