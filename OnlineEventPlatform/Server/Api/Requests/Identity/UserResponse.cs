namespace Api.Requests.Identity;

public class UserResponse
{
    public UserContainer User { get; set; } = null!;
    public class UserContainer
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime TokenValidTo { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
        public bool HasTwoFactoryAuthEnable { get; set; }
    }
}