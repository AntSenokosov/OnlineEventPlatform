using System.Net;
using Domain.Identity.Entities;
using Google.Authenticator;

namespace Infrastructure.Helpers;

public static class TwoFactorAuthHelper
{
    public static bool TwoFactorAuthentication(User user, string googleAuthCode)
    {
        if (user.IsGoogleAuthEnabled)
        {
            var tfa = new TwoFactorAuthenticator();
            bool result = tfa.ValidateTwoFactorPIN(user.GoogleAuthKey, googleAuthCode);
            if (!result)
            {
                throw new RestException(
                    HttpStatusCode.Unauthorized,
                    new
                    {
                        Error = "Failed to authenticate with Google Authenticator code. Please try again!"
                    });
            }
        }

        return true;
    }
}