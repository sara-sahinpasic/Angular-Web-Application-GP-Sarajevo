using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Application.Services.Implementations.Mapper;
using Domain.ViewModels;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.Account
{
    [ApiController]
    [Route("[controller]")]
    public sealed class ProfileController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public ProfileController(IUserRepository userRepository)
        {
            this._userRepository=userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
        {
            //ToDo: provjera da li je logiran

            var user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user == null)
            {
                return NotFound("Nema podataka");
            }
            return Ok(user);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateProfile(UserUpdateRequestDto vM, CancellationToken cancellationToken, 
            [FromServices]IUnitOfWork unitOfWork, [FromServices] ObjectMapperService objectMapperService)
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
        public async Task<IActionResult> DeleteProfile(Guid id, CancellationToken cancellationToken, [FromServices]IUnitOfWork unitOfWork)
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
