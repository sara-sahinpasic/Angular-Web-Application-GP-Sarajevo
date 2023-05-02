using Domain.Entities.Users;

namespace Application.Services.Abstractions.Interfaces.SMS;

public interface ISMSService
{
    Task<bool> SendVerificationCode(User user);
}
