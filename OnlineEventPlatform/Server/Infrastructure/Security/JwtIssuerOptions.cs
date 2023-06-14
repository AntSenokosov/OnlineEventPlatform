using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Security;

public class JwtIssuerOptions
{
    public const string Schemes = "Bearer";
    public string Issuer { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string Audience { get; set; } = null!;
    public DateTime NotBefore => DateTime.UtcNow;
    public DateTime IssuedAt => DateTime.UtcNow;
    public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(100);
    public DateTime Expiration => IssuedAt.Add(ValidFor);
    public Func<Task<string>> JtiGenerator => () => Task.FromResult(Guid.NewGuid().ToString());
    public SigningCredentials SigningCredentials { get; set; } = null!;
}