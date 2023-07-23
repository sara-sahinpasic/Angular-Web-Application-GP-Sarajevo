using Application.Services.Abstractions.Interfaces.Authentication;
using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
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

    [HttpGet]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken, [FromServices] IObjectMapperService objectMapperService)
    {
        //ToDo: provjera da li je logiran

        var user = await _userRepository.GetByIdAsync(id, cancellationToken);

        if (user == null)
        {
            return NotFound("Nema podataka");
        }
        var userDto = new UserProfileDto();
        objectMapperService.Map(user, userDto);
        return Ok(userDto);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile(UserUpdateRequestDto vM, CancellationToken cancellationToken,
        [FromServices] IUnitOfWork unitOfWork, [FromServices] IObjectMapperService objectMapperService,
        [FromServices] IAuthService authService)
    {
        //ToDo: provjera da li je logiran

        var data = await _userRepository.GetByIdAsync(vM.Id, cancellationToken);

        if (data == null)
        {
            Response<object?> errorResponse = new()
            {
                Message = "No user found",
                Data = null
            };

            return NotFound(errorResponse);
        }

        objectMapperService.Map(vM, data);

        _userRepository.Update(data);
        await unitOfWork.CommitAsync(cancellationToken);

        string token = HttpContext.Request.Headers["Authorization"];
        DateTime tokenIssuedAtDate = authService.GetJwtIssuedDateFromToken(token);
        string jwtToken = authService.GenerateJwtToken(data, tokenIssuedAtDate);

        Response<string> response = new()
        {
            Message = "User successfully updated!",
            Data = jwtToken
        };

        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteProfile(Guid id, CancellationToken cancellationToken, [FromServices] IUnitOfWork unitOfWork)
    {
        //ToDo: provjera da li je logiran

        var data = await _userRepository.GetByIdAsync(id, cancellationToken);
        if (data == null)
        {
            Response<object?> response = new()
            {
                Message = "No user found",
                Data = null
            };

            return NotFound(response);
        }

        _userRepository.Delete(data);
        await unitOfWork.CommitAsync(cancellationToken);

        Response<string> responseOk = new()
        {
            Message = "User successfully deleted!",
        };
        return Ok(responseOk);
    }
    
    [HttpGet("Status")]
    public async Task<IActionResult> GetAllStatuses([FromServices] IUserStatusRepository userStatusRepository)
    {
        var data = await userStatusRepository
            .GetAll()
            .Select(status => new UserStatusDto { Name = status.Name, Id=status.Id })
            .ToArrayAsync();

        Response<UserStatusDto[]> response = new()
        {
            Message = "",
            Data = data
        };
        return Ok(response);
    }
}

