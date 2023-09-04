using Application.Services.Abstractions.Interfaces.Repositories.Routes;
using Domain.Entities.Routes;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Routes;

public sealed class HolidayRepository : GenericRepository<Holiday>, IHolidayRepository
{
    public HolidayRepository(DataContext dataContext) : base(dataContext) {}

    public Task<bool> IsHolidayAsync(DateTime date, CancellationToken cancellationToken = default)
    {
        DateTime dateOnly = date.Date;

        return GetAll().AnyAsync(holiday => holiday.Date.Date == dateOnly, cancellationToken);
    }
}
