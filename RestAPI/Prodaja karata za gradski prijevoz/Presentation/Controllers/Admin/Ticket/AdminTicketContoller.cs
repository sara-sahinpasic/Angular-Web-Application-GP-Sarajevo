using Application.Services.Abstractions.Interfaces.Mapper;
using Microsoft.AspNetCore.Mvc;
using Presentation.DTO;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Presentation.DTO.Admin.Ticket;
using Application.Services.Abstractions.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Application.Config;

namespace Presentation.Controllers.Admin.Ticket
{
    [Authorize(Policy = AuthorizationPolicies.AdminPolicyName)]
    [ApiController]
    [Route("[controller]")]
    public sealed class AdminTicketContoller : ControllerBase
    {
        private readonly ITicketRepository ticketRepository;
        private readonly IObjectMapperService mapperService;
        private readonly IUnitOfWork unitOfWork;

        public AdminTicketContoller(ITicketRepository ticketRepository, IObjectMapperService mapperService, IUnitOfWork unitOfWork)
        {
            this.ticketRepository = ticketRepository;
            this.mapperService = mapperService;
            this.unitOfWork = unitOfWork;
        }

        [HttpPost("/Ticket")]
        public async Task<IActionResult> AddTicket(TicketDto ticketDto, CancellationToken cancellationToken)
        {
            bool doesTicketExistAsync = await ticketRepository.DoesNameTicketExistAsync(ticketDto.Name, cancellationToken);
            if (doesTicketExistAsync)
            {
                Response invalidResponse = new()
                {
                    Message = "Karta u sistemu već postoji."
                };
                return BadRequest(invalidResponse);
            }

            Domain.Entities.Tickets.Ticket newTicket = new();
            mapperService.Map(ticketDto, newTicket);

            ticketRepository.Create(newTicket);
            await unitOfWork.CommitAsync(cancellationToken);

            Response response = new()
            {
                Message = "Nova karta kreirana.",
                Data = newTicket
            };
            return Ok(response);
        }
    }
}
