using Application.Services.Abstractions.Interfaces.Driver;
using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.Repositories.Tickets;
using Application.Services.Abstractions.Interfaces.Repositories.Vehicles;
using Domain.Entities.Driver;
using Domain.Entities.Users;
using Domain.Entities.Vehicles;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Driver;

public class MalfunctionService : IMalfunctionService
{
    private readonly IIssuedTicketRepository _issuedTicketRepository;
    private readonly IEmailService _emailService;
    private readonly IVehicleRepository _vehicleRepository;

    public MalfunctionService(IIssuedTicketRepository issuedTicketRepository, IEmailService emailService, 
        IVehicleRepository vehicleRepository)
    {
        _issuedTicketRepository = issuedTicketRepository;
        _emailService = emailService;
        _vehicleRepository = vehicleRepository;
    }

    public async Task SendFixedMalfunctionNotification(Vehicle vehicle, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IReadOnlyCollection<User> users = await _issuedTicketRepository.GetAll()
            .Where(issuedTicket => issuedTicket.Route.VehicleId == vehicle.Id)
            .Where(issuedTicket => DateTime.Now.Date <= issuedTicket.ValidFrom.Date)
            .Select(issuedTicket => issuedTicket.User)
            .Distinct()
            .ToListAsync(cancellationToken);

        string subject = string.Format("Kvar vozila broj {0} otklonjen/Malfunction for vehicle number {0} is fixed", vehicle.Number);
        string content = "Vozilo koje prevozi Vašu rutu je popravljeno." +
            "<br /> <br /> The vehicle that is driving your route has been fixed.";

        foreach (var user in users)
        {
            await _emailService.SendNoReplyMailAsync(user, subject, content, cancellationToken);
        }
    }

    public async Task SendMalfunctionNotification(Malfunction malfunction, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        IReadOnlyCollection<User> users = await _issuedTicketRepository.GetAll()
            .Where(issuedTicket => issuedTicket.Route.VehicleId == malfunction.VehicleId)
            .Where(issuedTicket => DateTime.Now.Date <= issuedTicket.IssuedDate.Date)
            .Select(issuedTicket => issuedTicket.User)
            .Distinct()
            .ToListAsync(cancellationToken);

        Vehicle vehicle = await _vehicleRepository.GetByIdEnsuredAsync(malfunction.VehicleId, cancellationToken: cancellationToken);

        string subject = string.Format("Kvar vozila broj {0}/Malfunction for vehicle number {0}", vehicle.Number);
        string content = "Vozilo koje prevozi Vašu rutu je u kvaru te neće moći izvršiti prevoz. <br />Refundaciju karte možete zatražiti na refundiraj_me@mail.com." +
            "<br /> <br /> The vehicle that is driving your route has a malfunction and will not be able to carry out the transport.<br/>You can request for refunds at refundiraj_me@mail.com.";

        foreach (var user in users)
        {
            await _emailService.SendNoReplyMailAsync(user, subject, content, cancellationToken);
        }
    }
}
