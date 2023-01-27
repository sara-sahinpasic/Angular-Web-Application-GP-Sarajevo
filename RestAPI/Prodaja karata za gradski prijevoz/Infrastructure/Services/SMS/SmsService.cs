using Application.Config.SMS;
using Application.Services.Abstractions.Interfaces.Repositories.Korisnici;
using Application.Services.Abstractions.Interfaces.SMS;
using Domain.Entities.Korisnici;
using Telesign;

namespace Infrastructure.Services.SMS;

public sealed class SmsService : ISMSService
{
    private readonly SMSConfig _smsConfig;
    private readonly IVerificationCodeRepository _verificationCodeRepository;

    public SmsService(SMSConfig smsConfig, IVerificationCodeRepository verificationCodeRepository)
    {
        _smsConfig = smsConfig;
        _verificationCodeRepository = verificationCodeRepository;
    }

    public async Task<bool> SendVerificationCode(Korisnik user)
    {
        string phoneNumber = "38762924925";
        int verificationCode = Random.Shared.Next(1000, 9999);
        string message = $"Your verification code {verificationCode}";
        string messageType = "ARN";

        using MessagingClient client = new(_smsConfig.CustomerId, _smsConfig.APIKey, _smsConfig.Endpoint);
        RestClient.TelesignResponse telesignResponse = await client.MessageAsync(phoneNumber, message, messageType);
        
        return telesignResponse.OK;
    }
}
