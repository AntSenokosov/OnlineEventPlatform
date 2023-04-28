namespace Infrastructure.Security;

public interface IPasswordHasher
{
    public byte[] Hash(string password, byte[] salt);
}