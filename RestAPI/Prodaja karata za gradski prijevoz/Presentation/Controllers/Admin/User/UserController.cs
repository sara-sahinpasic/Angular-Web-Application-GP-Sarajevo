using Application.Config;
using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Requests;
using Application.Services.Abstractions.Interfaces.Repositories.Roles;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.Admin.User;
using UserEntity = Domain.Entities.Users.User;

namespace Presentation.Controllers.Admin.AdminUsers;

[Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
[ApiController]
[Route("Admin/[controller]")]
public sealed class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UserController(IUserRepository userRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("Roles/All")]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
    {
        var data = await _roleRepository.GetAll()
            .ToArrayAsync(cancellationToken);

        Response response = new()
        {
            Data = data
        };

        return Ok(response);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> CreateUser(CreateUserDto createDto, [FromServices] IPasswordService passwordService,
        CancellationToken cancellationToken)
    {
        if (await _userRepository.IsUserRegisteredAsync(createDto.Email))
        {
            Response errorResponse = new()
            {
                Message = "Email već postoji u bazi podataka."
            };

            return BadRequest(errorResponse);
        }

        Tuple<byte[], string> passwordHashAndSalt = passwordService.GeneratePasswordHashAndSalt(createDto.Password);
        UserEntity newUser = new UserEntity
        {
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Email = createDto.Email,
            PasswordHash = passwordHashAndSalt.Item2,
            PasswordSalt = passwordHashAndSalt.Item1,
            DateOfBirth = createDto.DateOfBirth,
            RoleId = createDto.RoleId,
            RegistrationDate = DateTime.Now,
            PhoneNumber = createDto.PhoneNumber,
            ModifiedDate = DateTime.Now
        };

        await _userRepository.CreateAsync(newUser, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response response = new()
        {
            Message = "Novi korisnik registrovan.",
            Data = newUser
        };

        return Ok(response);
    }

    [HttpGet("All")]
    public async Task<IActionResult> GetAllUsers([FromServices] IRequestRepository requestRepository, CancellationToken cancellationToken)
    {
        var data = await _userRepository.GetAll()
            .Select(user => new GetUserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                RoleName = user.Role.Name,
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                HasPendingRequest = requestRepository.GetAll().Where(request => request.UserId == user.Id && request.Active).Any()
            })
            .ToArrayAsync(cancellationToken);

        Response response = new()
        {
            Data = data
        };

        return Ok(response);
    }

    [HttpGet("Get/{id}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken, [FromServices] IObjectMapperService objectMapperService)
    {
        var data = await _userRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (data == null)
        {
            Response errorResponse = new()
            {
                Message = "Korisnik nije pronađen"
            };

            return NotFound(errorResponse);
        }

        var user = new GetUserDto();
        objectMapperService.Map(data, user);

        Response response = new()
        {
            Data = user
        };

        return Ok(response);
    }
}