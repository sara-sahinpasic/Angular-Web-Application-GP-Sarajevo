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
    [Route("Admin/[controller]")]
    public sealed class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IObjectMapperService _mapperService;
        private readonly IUnitOfWork _unitOfWork;

        public TicketController(ITicketRepository ticketRepository, IObjectMapperService mapperService, IUnitOfWork unitOfWork)
        {
            _ticketRepository = ticketRepository;
            _mapperService = mapperService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("Add")]
        public async Task<IActionResult> AddTicket(TicketDto ticketDto, CancellationToken cancellationToken)
        {
            bool doesTicketExistAsync = await _ticketRepository.DoesNameTicketExistAsync(ticketDto.Name, cancellationToken);
            if (doesTicketExistAsync)
            {
                Response invalidResponse = new()
                {
                    Message = "Karta u sistemu već postoji."
                };
                return BadRequest(invalidResponse);
            }

            Domain.Entities.Tickets.Ticket newTicket = new();
            _mapperService.Map(ticketDto, newTicket);

            await _ticketRepository.CreateAsync(newTicket, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            Response response = new()
            {
                Message = "Nova karta kreirana.",
                Data = newTicket
            };
            
            return Ok(response);
        }
    }
}
