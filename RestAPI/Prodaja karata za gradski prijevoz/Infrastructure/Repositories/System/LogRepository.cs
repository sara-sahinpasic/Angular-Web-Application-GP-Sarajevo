using Application.Services.Abstractions.Interfaces.Repositories;
using Application.Services.Abstractions.Interfaces.Repositories.System;
using Domain.Entities.System;
using Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repositories.System;

public sealed class LogRepository : GenericRepository<Log>, ILogRepository
{
    public LogRepository(DataContext dataContext) : base(dataContext) {}
}
