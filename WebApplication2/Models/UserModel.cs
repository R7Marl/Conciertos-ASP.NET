namespace WebApplication2.models
{
    public class UserModel
    {
        public string name { get; set; } = string.Empty;

        public string ?role { get; set; } = string.Empty;
        public string email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;

    }
    public class UserLogin
    {
        public string Email { get; set; } = "email";
        public string Password { get; set; } = "password";
    }
}
