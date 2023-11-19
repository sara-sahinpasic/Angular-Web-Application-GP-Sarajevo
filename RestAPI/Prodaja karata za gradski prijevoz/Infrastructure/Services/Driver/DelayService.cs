using Application.Services.Abstractions.Interfaces.Driver;
using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.Repositories.Routes;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Domain.Entities.Driver;
using Domain.Entities.Routes;
using Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Driver;

public class DelayService : IDelayService
{
    private readonly IEmailService _emailService;
    private readonly IIssuedTicketRepository _issuedTicketRepository;
    private readonly IRouteRepository _routeRepository;

    public DelayService(IEmailService emailService, IIssuedTicketRepository issuedTicketRepository, IRouteRepository routeRepository)
    {
        _emailService = emailService;
        _issuedTicketRepository = issuedTicketRepository;
        _routeRepository = routeRepository;
    }

    public async Task SendDelayNotification(Delay delay, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IReadOnlyCollection<User> users = await _issuedTicketRepository.GetAll()
            .Where(issuedTicket => issuedTicket.RouteId == delay.RouteId && issuedTicket.ValidFrom.Date == DateTime.Now.Date)
            .Select(issuedTicket => issuedTicket.User)
            .Distinct()
            .ToListAsync(cancellationToken);

        Route route = await _routeRepository.GetByIdEnsuredAsync(delay.RouteId, new[] { "StartStation", "EndStation" }, cancellationToken);

        string subject = "Kašnjenje/Delay";
        string content = string.Format("Vaš prevoz za rutu {0} - {1} u {2} - {3} će kasniti {4} minuta. <br /><br/ >The transport for the route {0} - {1} at {2} - {3} will be delayed for {4} minutes.",
            route.StartStation.Name, route.EndStation.Name, route.TimeOfDeparture, route.TimeOfArrival, delay.DelayAmount);

        foreach (User user in users)
        {
            await _emailService.SendNoReplyMailAsync(user, subject, content, cancellationToken);
        }
    }
}
