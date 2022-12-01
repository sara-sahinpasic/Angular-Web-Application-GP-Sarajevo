using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace Application.Helpers;

public static class EncryptionDecryptionHelper
{
    // Todo: maybe create as a service?
    public static string GenerateRegistrationToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray());
    }
}
