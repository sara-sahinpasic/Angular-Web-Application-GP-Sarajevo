namespace Application.Services.Abstractions.Interfaces.Requests;

public interface IRequestService
{
    public Task ApproveRequest(Guid requestId, DateTime expirationDate, CancellationToken cancellationToken);
    public Task<bool> RejectRequest(Guid requestId, string rejectionReason, CancellationToken cancellationToken);
}
