using Application.Services.Abstractions.Interfaces.Authentication;
using Application.Services.Abstractions.Interfaces.File;
using Application.Services.Abstractions.Interfaces.Hashing;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Presentation.DTO;
using Presentation.DTO.User;

namespace Presentation.Controllers.Account;

[Authorize]
[ApiController]
[Route("[controller]")]

public sealed class ProfileController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public ProfileController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromForm] UserUpdateRequestDto vM, CancellationToken cancellationToken,
        [FromServices] IUnitOfWork unitOfWork, [FromServices] IObjectMapperService objectMapperService,
        [FromServices] IAuthService authService, [FromServices] IFileService fileService,
        [FromServices] IPasswordService passwordService)
    {
        var data = await _userRepository.GetByIdAsync(vM.Id, cancellationToken);

        if (data == null)
        {
            Response errorResponse = new()
            {
                Message = "profile_controller_update_profile_no_user_found"
            };

            return NotFound(errorResponse);
        }

        if (vM.ProfileImageFile is not null)
        {
            string? filePath = await fileService.SaveFileAsync(new[] { "jpg", "jpeg", "png" }, vM.ProfileImageFile, "ProfileImages", cancellationToken);

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

        objectMapperService.Map(vM, data);

        if (vM.Password is not null && vM.Password.Length >= 8)
        {
            Tuple<byte[], string> password = passwordService.GeneratePasswordHashAndSalt(vM.Password);

            data.PasswordHash = password.Item2;
            data.PasswordSalt = password.Item1;
        }

        _userRepository.Update(data);
        await unitOfWork.CommitAsync(cancellationToken);

        string token = HttpContext.Request.Headers["Authorization"];
        DateTime tokenIssuedAtDate = authService.GetJwtIssuedDateFromToken(token);
        string jwtToken = authService.GenerateJwtToken(data, tokenIssuedAtDate);

        Response responseOk = new()
        {
            Message = "profile_controller_update_profile_update_success",
            Data = jwtToken
        };

        return Ok(responseOk);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProfile(Guid id, CancellationToken cancellationToken, [FromServices] IUnitOfWork unitOfWork)
    {
        var data = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (data == null)
        {
            Response response = new()
            {
                Message = "profile_controller_delete_profile_no_user_found",
                Data = null
            };

            return NotFound(response);
        }

        _userRepository.Delete(data);
        await unitOfWork.CommitAsync(cancellationToken);

        Response responseOk = new()
        {
            Message = "profile_controller_delete_profile_success",
        };
        return Ok(responseOk);
    }

    [HttpGet("Status")]
    public async Task<IActionResult> GetAllStatuses([FromServices] IUserStatusRepository userStatusRepository)
    {
        var data = await userStatusRepository
            .GetAll()
            .Select(status => new UserStatusDto { Name = status.Name, Id = status.Id })
            .ToArrayAsync();

        Response response = new()
        {
            Data = data
        };

        return Ok(response);
    }

    [HttpGet("UserImage/{id}")]
    public async Task<IActionResult> GetUserImage(Guid id)
    {
        string path = _userRepository.GetAll().Where(x => x.Id == id).Select(x => x.ProfileImagePath).First();

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

