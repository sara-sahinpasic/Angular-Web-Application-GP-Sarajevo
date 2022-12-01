using Application.Abstractions.Email;
using Application.Helpers;
using Domain.Abstractions.Interfaces.Korisnici;
using Domain.DTO.Korisnik;
using Domain.Entities.Korisnici;
using Microsoft.AspNetCore.Mvc;

namespace Prodaja_karata_za_gradski_prijevoz.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(ILogger<RegistrationController> logger)
        {
            _logger = logger;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> RegisterAction(
            [FromServices] IEmailHandler emailHandler, 
            [FromServices] IKorisnikRepozitorij korisnikRepozitorij,
            [FromServices] IRegistracijskiTokenRepository registracijskiTokenRepository,
            KorisnikRequest userRequest)
        {
            // todo: middleware for exception handling
            try
            {
                Korisnik user = new();
                ObjectMapper.Map(userRequest, user);

                Guid? userId = await korisnikRepozitorij.Create(user);
                RegistracijskiToken? token = await registracijskiTokenRepository.Create(user);
                await emailHandler.SendNoReplyMail(user, 
                    "Testiranje", $"http://localhost:5192/account/activate/{token?.Token}");

                return CreatedAtAction(nameof(RegisterAction), userId);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("/account/activate/{tokenString}")]
        public async Task<IActionResult> ActivateAction(
            [FromServices] IKorisnikRepozitorij korisnikRepozitorij,
            [FromServices] IRegistracijskiTokenRepository registracijskiTokenRepository,
            string tokenString
        )
        {
            var token = await registracijskiTokenRepository.GetInactiveByTokenString(tokenString);

            if (token is null)
                return BadRequest("Token is either already activated or it is expired.");

            Korisnik? user = token.Korisnik;
            user!.Aktivan = true;

            token.Aktiviran = true;

            await registracijskiTokenRepository.Update(token);
            await korisnikRepozitorij.Update(user);

            return Ok();
        }
    }
}