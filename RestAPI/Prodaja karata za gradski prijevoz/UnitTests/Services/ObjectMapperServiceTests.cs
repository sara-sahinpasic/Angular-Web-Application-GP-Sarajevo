using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Implementations.Mapper;
using Domain.Entities.Korisnici;

namespace UnitTests.Services;

public sealed class ObjectMapperServiceTests
{
    private readonly IObjectMapperService _objecMapperService;

    public ObjectMapperServiceTests()
    {
        _objecMapperService = new ObjectMapperService();
    }

    [Fact]
    public void Map_ShouldReturnMappedObject_WhenPassedObjectNotNull()
    {
        Korisnik user = new()
        {
            Ime = "amor",
            Prezime = "osmic"
        };

        Korisnik userToMapTo = new();

        _objecMapperService.Map(user, userToMapTo);

        Assert.Equal(user.Ime, userToMapTo.Ime);
        Assert.Equal(user.Prezime, userToMapTo.Prezime);
    }

    [Fact]
    public void Map_ShouldThrowNullArgumentException_WhenPassedObjectIsNull()
    {
        Korisnik? user = null;

        Korisnik userToMapTo = new();

        Assert.Throws<ArgumentNullException>(() => _objecMapperService.Map(user, userToMapTo));
    }
}