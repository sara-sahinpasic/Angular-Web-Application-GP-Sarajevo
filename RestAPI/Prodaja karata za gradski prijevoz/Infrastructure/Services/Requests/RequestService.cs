using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.Repositories.Requests;
using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Requests;
using Domain.Entities.Requests;
using UserEntity = Domain.Entities.Users.User;

namespace Infrastructure.Services.Requests;

public sealed class RequestService : IRequestService
{
    private readonly IEmailService _emailService;
    private readonly IRequestRepository _requestRepository;
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RequestService(IEmailService emailService, IRequestRepository requestRepository, 
        IUserRepository userRepository, IUnitOfWork unitOfWork)
    {
        _emailService = emailService;
        _requestRepository = requestRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task ApproveRequest(Guid requestId, DateTime expirationDate, CancellationToken cancellationToken)
    {
        Request request = await _requestRepository.GetByIdEnsuredAsync(requestId, new[] { "User", "UserStatus" }, cancellationToken);
        UserEntity user = request.User;

        request.Active = false;
        request.Approved = true;

        user.UserStatusId = request.UserStatusId;
        user.StatusExpirationDate = expirationDate;

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _requestRepository.UpdateAsync(request, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        string subject = $"Odobren zahtjev za status {request.UserStatus.Name}";
        string formattedDate = user.StatusExpirationDate.Value.ToString("dd.MM.yyyy");
        string content = $"Poštovani {user.FirstName} {user.LastName}, vaš zahtjev za status {request.UserStatus.Name} je odboren. Važi do {formattedDate}";

        await _emailService.SendNoReplyMailAsync(user, subject, content, cancellationToken);
    }

    public async Task<bool> RejectRequest(Guid requestId, string rejectionReason, CancellationToken cancellationToken)
    {
        Request request = await _requestRepository.GetByIdEnsuredAsync(requestId, new[] { "User", "UserStatus" }, cancellationToken);

        if (!request.Active)
        {
            return false;
        }

        UserEntity user = request.User;

        request.Active = false;
        request.Approved = false;
        request.RejectionReason = rejectionReason;

        user.UserStatusId = null;
        user.StatusExpirationDate = null;

        await _userRepository.UpdateAsync(user, cancellationToken);
        await _requestRepository.UpdateAsync(request, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        string subject = $"Odbijen zahtjev za status {request.UserStatus.Name}";
        string content = rejectionReason;

        await _emailService.SendNoReplyMailAsync(user, subject, content, cancellationToken);

        return true;
    }
}
