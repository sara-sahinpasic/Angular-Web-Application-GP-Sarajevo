using Microsoft.Extensions.Logging;

namespace Application.Services.Abstractions.Interfaces.System;

public interface ILogService
{
    public Task LogAsync(string message, LogLevel logLevel);
}
