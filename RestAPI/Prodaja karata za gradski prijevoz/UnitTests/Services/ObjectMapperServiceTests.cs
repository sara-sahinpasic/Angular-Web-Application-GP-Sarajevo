using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Implementations.Mapper;
using Domain.Entities.Users;
using Domain.Enums.User;

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
        User user = new()
        {
            FirstName = "amor",
            LastName = "osmic",
            Status = null
        };

        User userToMapTo = new();

        _objecMapperService.Map(user, userToMapTo);

        Assert.Equal(user.FirstName, userToMapTo.FirstName);
        Assert.Equal(user.LastName, userToMapTo.LastName);
    }

    [Fact]
    public void Map_ShouldThrowNullArgumentException_WhenPassedObjectIsNull()
    {
        User? user = null;

        User userToMapTo = new();

        Assert.Throws<ArgumentNullException>(() => _objecMapperService.Map(user, userToMapTo));
    }
}