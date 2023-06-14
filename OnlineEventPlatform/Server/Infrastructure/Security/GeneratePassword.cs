using System.Security.Cryptography;

namespace Infrastructure.Security;

public class GeneratePassword : IGeneratePassword
{
    private const string ValidChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
    private const int Length = 10;
    [Obsolete("Obsolete")]
    public string GenerateRandomPassword()
    {
        var password = new char[Length];

        using (var cryptoProvider = new RNGCryptoServiceProvider())
        {
            var randomBytes = new byte[Length];

            cryptoProvider.GetBytes(randomBytes);

            for (var i = 0; i < Length; i++)
            {
                password[i] = ValidChars[randomBytes[i] % ValidChars.Length];
            }
        }

        return new string(password);
    }
}