using Application.Services.Abstractions.Interfaces.Mapper;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO.User;

namespace Presentation.Controllers.Account
{
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
            [FromServices] IUnitOfWork unitOfWork, [FromServices] IObjectMapperService objectMapperService)
        {
            //ToDo: provjera da li je logiran

            var data = await _userRepository.GetByIdAsync(vM.Id, cancellationToken);

            if (data == null)
            {
                return NotFound("Nema podataka");
            }

            objectMapperService.Map(vM, data);

            _userRepository.Update(data);
            await unitOfWork.CommitAsync(cancellationToken);
            return Ok(data);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProfile(Guid id, CancellationToken cancellationToken, [FromServices] IUnitOfWork unitOfWork)
        {
            //ToDo: provjera da li je logiran

            var data = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (data == null)
            {
                return NotFound("Nema podataka");
            }

            _userRepository.Delete(data);
            await unitOfWork.CommitAsync(cancellationToken);
            return Ok(data);
        }
    }
}
