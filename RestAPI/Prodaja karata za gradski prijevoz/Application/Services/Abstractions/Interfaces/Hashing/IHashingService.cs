namespace Application.Services.Abstractions.Interfaces.Hashing;

public interface IHashingService
{
    public Tuple<byte[], string> GeneratePasswordHashAndSalt(string password);
    public bool VerifyPasswordHash(string password, string passwordHash, byte[] passwordSalt);
}
