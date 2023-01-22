using Application.Services.Abstractions.Interfaces.Hashing;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services.Implementations.Hashing;

public sealed class HashingService : IHashingService
{
    // todo: create seperate service for hashing as it is not the same as encryption
    public Tuple<byte[], string> GeneratePasswordHashAndSalt(string password)
    {
        byte[] salt = new byte[256 / 8];
        byte[] hashedPassword = new byte[256 / 8];

        using (HMACSHA256 hmac = new())
        {
            salt = hmac.Key;
            hashedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        return new Tuple<byte[], string>(salt, Encoding.UTF8.GetString(hashedPassword));
    }

    // todo: should be a token service?
    public string GenerateRegistrationToken()
    {
        return Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Replace("/", "_");
    }

    public bool VerifyPasswordHash(string password, string passwordHash, byte[] passwordSalt)
    {
        byte[] passwordHashToCheck;

        using (HMACSHA256 hmac = new()) 
        {
            hmac.Key = passwordSalt;
            passwordHashToCheck = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));       
        }

        return Encoding.UTF8.GetString(passwordHashToCheck).Equals(passwordHash);
    }
}
