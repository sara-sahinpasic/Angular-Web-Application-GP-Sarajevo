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
        private readonly DataContext _dbContext;
        public ProfileController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetByPhoneNumber(Guid id)
        {
            //ToDo: provjera da li je logiran

            var user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
            {
                return NotFound("Nema podataka");
            }
            return Ok(user);
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> UpdateProfile(KorisnikVM vM)
        {
            //ToDo: provjera da li je logiran

            var data = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == vM.Id);
            if (data == null)
            {
                return NotFound("Nema podataka");
            }

            data.Ime = vM.Ime;
            data.Prezime = vM.Prezime;
            data.BrojTelefona = vM.BrojTelefona;
            data.Adresa = vM.Adresa;

            _dbContext.Users.Update(data);
            await _dbContext.SaveChangesAsync();
            return Ok(data);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProfile(Guid id)
        {
            //ToDo: provjera da li je logiran

            var data = await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                return NotFound("Nema podataka");
            }

            _dbContext.Users.Remove(data);
            await _dbContext.SaveChangesAsync();
            return Ok(data);
        }
    }
}
