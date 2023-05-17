using Application.Services.Abstractions.Interfaces.Hashing;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services.Implementations.Hashing;

public sealed class PasswordService : IPasswordService
{
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

    public string GenerateRandomPassword()
    {
        int length = 10;
        StringBuilder stringBuilder = new();

        for (int i = 0; i < length; i++)
        {
            stringBuilder.Append((char)RandomNumberGenerator.GetInt32(33, 126));
        }

        return stringBuilder.ToString();
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
