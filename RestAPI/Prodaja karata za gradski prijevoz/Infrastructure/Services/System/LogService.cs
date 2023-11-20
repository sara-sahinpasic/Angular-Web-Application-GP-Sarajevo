using Application.Services.Abstractions.Interfaces.Email;
using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.System;
using Application.Services.Abstractions.Interfaces.System;
using Domain.Entities.System;
using Domain.Entities.Users;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.System;

public sealed class LogService : ILogService
{
    private readonly ILogRepository _logRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public LogService(IUnitOfWork unitOfWork, ILogRepository logRepository, IEmailService emailService, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _logRepository = logRepository;
        _emailService = emailService;
        _configuration = configuration;
    }

    public async Task LogAsync(Exception exception, LogLevel logLevel)
    {
        if (!IsValidLogLevel(logLevel))
        {
            return;
        }

        Log log = new()
        {
            Level = logLevel.ToString(),
            Message = exception.Message,
            InnerMessage = exception.InnerException?.Message,
            Date = DateTime.UtcNow
        };

        await _logRepository.CreateAsync(log);

        bool shouldSendLogMail = _configuration.GetValue<bool>("SendErrorLogsToEmail");

        if (shouldSendLogMail) 
        {
            await SendLogMail(log);
        }

        await _unitOfWork.CommitAsync(default);
    }

    private static bool IsValidLogLevel(LogLevel logLevel) => logLevel == LogLevel.Error || logLevel == LogLevel.Critical;

    private Task SendLogMail(Log log)
    {
        string mailContent = $"An error occured on { log.Date } UTC. Error message \"{ log.Message }\"";
        string subject = "Error on system";

        User user = new()
        {
            FirstName = _configuration["System:AdminFirstName"],
            LastName = _configuration["System:AdminLastName"],
            Email = _configuration["System:AdminEmail"]
        };

        return _emailService.SendNoReplyMailAsync(user, subject, mailContent, default);
    }
}
