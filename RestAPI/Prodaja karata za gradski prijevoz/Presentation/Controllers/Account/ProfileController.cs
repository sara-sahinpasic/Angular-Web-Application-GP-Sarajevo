using Application.Config;
using Application.Services.Abstractions.Interfaces.Authentication;
using Application.Services.Abstractions.Interfaces.File;
using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Application.Validators.User;
using Domain.Entities.Users;
using Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.User;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;

namespace Presentation.Controllers.Account;

[Authorize(Policy = AuthorizationPolicies.AdminUserPolicyName)]
[ApiController]
[Route("[controller]")]
public sealed class ProfileController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    
    public ProfileController(IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromForm] UserUpdateRequestDto userUpdateRequest,
        [FromServices] IObjectMapperService objectMapperService, [FromServices] IAuthService authService,
        [FromServices] IFileService fileService, [FromServices] IPasswordService passwordService,
        CancellationToken cancellationToken)
    {
        string authorizationHeaderValue = Request.Headers["Authorization"];
        string token = authorizationHeaderValue.Split(" ")[1];

        JwtSecurityToken jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        if (!IdentityValidator.IsSameUser(userUpdateRequest.Id, jwtSecurityToken) && !IdentityValidator.IsUserRole("admin", jwtSecurityToken))
        {
            return Forbid();
        }

        string[] includes = { "Role" };
        User? data = await _userRepository.GetByIdAsync(userUpdateRequest.Id, includes, cancellationToken);

        if (data == null)
        {
            Response errorResponse = new()
            {
                Message = "profile_controller_update_profile_no_user_found"
            };

            return NotFound(errorResponse);
        }

        if (userUpdateRequest.ProfileImageFile is not null)
        {
            string? filePath = await fileService.SaveFileAsync(new[] { "jpg", "jpeg", "png" }, userUpdateRequest.ProfileImageFile, "ProfileImages", cancellationToken);

            if (filePath is null)
            {
                Response errorResponse = new()
                {
                    Message = "profile_controller_update_profile_file_extension_error",
                };
                
                return NotFound(errorResponse);
            }

            data.ProfileImagePath = filePath;
        }

        objectMapperService.Map(userUpdateRequest, data);

        if (userUpdateRequest.Password is not null && userUpdateRequest.Password.Length >= 8)
        {
            Tuple<byte[], string> password = passwordService.GeneratePasswordHashAndSalt(userUpdateRequest.Password);

            data.PasswordHash = password.Item2;
            data.PasswordSalt = password.Item1;
        }

        await _userRepository.UpdateAsync(data, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        JsonElement? jwtToken = null;

        if (IdentityValidator.IsSameUser(userUpdateRequest.Id, jwtSecurityToken))
        {
            jwtToken = await authService.GetAuthTokenAsync(data, cancellationToken);
        }

        Response responseOk = new()
        {
            Message = "profile_controller_update_profile_update_success",
            Data = jwtToken
        };

        return Ok(responseOk);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProfile(Guid id, CancellationToken cancellationToken)
    {
        string authorizationHeaderValue = Request.Headers["Authorization"];
        string token = authorizationHeaderValue.Split(" ")[1];

        JwtSecurityToken jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);

        if (!IdentityValidator.IsSameUser(id, jwtSecurityToken) && !IdentityValidator.IsUserRole("admin", jwtSecurityToken))
        {
            return Forbid();
        }

        var user = await _userRepository.GetByIdAsync(id, cancellationToken: cancellationToken);

        if (user == null)
        {
            Response response = new()
            {
                Message = "profile_controller_delete_profile_no_user_found",
                Data = null
            };

            return NotFound(response);
        }

        await _userRepository.DeleteAsync(user, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        Response responseOk = new()
        {
            Message = "profile_controller_delete_profile_success",
        };
        
        return Ok(responseOk);
    }

    [HttpGet("Status")]
    public async Task<IActionResult> GetAllStatuses([FromServices] IUserStatusRepository userStatusRepository)
    {
        var userStatuses = await userStatusRepository
            .GetAll()
            .Select(status => new UserStatusDto { Name = status.Name, Id = status.Id })
            .ToArrayAsync();

        Response response = new()
        {
            Data = userStatuses
        };

        return Ok(response);
    }

    [HttpGet("UserImage/{id}")]
    public async Task<IActionResult> GetUserImage(Guid id)
    {
        string? path = await _userRepository.GetAll()
            .Where(x => x.Id == id)
            .Select(x => x.ProfileImagePath)
            .FirstOrDefaultAsync();

        if (string.IsNullOrEmpty(path))
        {
            return NotFound();
        }

        byte[] bytes = await System.IO.File.ReadAllBytesAsync(path);
        string base64 = Convert.ToBase64String(bytes);

        string[] pathParts = path.Split(".");
        string extension = pathParts[pathParts.Length - 1];
        
        Response responseOk = new()
        {
            Data = $"data:image/{extension};base64, {base64}"
        };
        
        return Ok(responseOk);
    }
}

