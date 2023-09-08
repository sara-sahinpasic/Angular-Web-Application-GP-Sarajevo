using Application.Services.Abstractions.Interfaces.Repositories.Roles;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Microsoft.EntityFrameworkCore;
using Domain.Entities.Users;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Authentication;
using Presentation.DTO.Admin.User;
using Application.Services.Abstractions.Interfaces.Repositories;

namespace Presentation.Controllers.Admin.AdminUsers
{

    [ApiController]
    [Route("[controller]")]
    public sealed class AdminUserContoller : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public AdminUserContoller(IUserRepository userRepository,
            IRoleRepository roleRepository, IAuthService authService,
            IUnitOfWork unitOfWork
            )
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _authService = authService;
            _unitOfWork = unitOfWork;
        }
        [HttpGet("/Roles")]
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
        [HttpPost("/CreateUser")]
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
            User newUser = new User
            {
                FirstName = createDto.FirstName,
                LastName = createDto.LastName,
                Email = createDto.Email,
                PasswordHash = passwordHashAndSalt.Item2,
                PasswordSalt = passwordHashAndSalt.Item1,
                DateOfBirth = (DateTime)createDto.DateOfBirth,
                RoleId = (Guid)createDto.RoleId,
                RegistrationDate = DateTime.UtcNow
            };

            _userRepository.Create(newUser);
            await _unitOfWork.CommitAsync(cancellationToken);

            Response response = new()
            {
                Message = "Novi korisnik registrovan.",
                Data = newUser
            };
            return Ok(response);
        }

        [HttpGet("/Users")]
        public async Task<IActionResult> GetAllUsers()
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
                Address = user.Address,
            })
           .ToArrayAsync();

            Response response = new()
            {
                Data = data
            };

            return Ok(response);
        }

        [HttpGet("/UserById")]
        public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken, [FromServices] IObjectMapperService objectMapperService)
        {
            var data = await _userRepository.GetByIdAsync(id, cancellationToken);

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
}
