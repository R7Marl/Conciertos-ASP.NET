namespace WebApplication2.Entity
{
    public class Users
    {
            public int id { get; set; }
            public string name { get; set; } = string.Empty;
            public string role { get; set; } = "user";
            public string email { get; set; } = string.Empty;
            public string password { get; set; } = string.Empty;
    }
}
