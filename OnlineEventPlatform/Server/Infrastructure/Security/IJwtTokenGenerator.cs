namespace Infrastructure.Security;

public interface IJwtTokenGenerator
{
    public Task<string> CreateToken(string email, int expiresAfterMinutes);
}