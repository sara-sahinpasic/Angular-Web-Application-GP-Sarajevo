using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Implementations.Hashing;

namespace UnitTests.Services;

public sealed class HashServiceTests
{
    private readonly IPasswordService _hashingService;

    public HashServiceTests()
    {
        _hashingService = new PasswordService();
    }

    [Theory]
    [InlineData("somepassword")]
    [InlineData("someotherpassword")]
    public void GeneratePasswordHashAndSalt_ShouldReturnTuplePasswordHashAndSalt_WhenGivenPasswordString(string password)
    {
        var result = _hashingService.GeneratePasswordHashAndSalt(password);
        string passwordHash = result.Item2;
        byte[] passwordSalt = result.Item1;

        Assert.NotEqual(password, passwordHash);
        Assert.NotEmpty(passwordSalt);
    }

    [Fact]
    public void GeneratePasswordHashAndSalt_ShouldThrowEmtpyStringException_WhenGivenEmptyPasswordString()
    {
        string password = "";

        Assert.Throws<ArgumentException>(() => _hashingService.GeneratePasswordHashAndSalt(password));
    }
}