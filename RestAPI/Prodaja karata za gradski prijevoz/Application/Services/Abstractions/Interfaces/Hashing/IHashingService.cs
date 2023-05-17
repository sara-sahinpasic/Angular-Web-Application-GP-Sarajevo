namespace Application.Services.Abstractions.Interfaces.Hashing;

public interface IPasswordService
{
    public Tuple<byte[], string> GeneratePasswordHashAndSalt(string password);
    public bool VerifyPasswordHash(string password, string passwordHash, byte[] passwordSalt);
    public string GenerateRandomPassword();
}
