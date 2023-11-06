using Application.Services.Abstractions.Interfaces.Repositories.Routes;
using Domain.Entities.Routes;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Routes;

public sealed class RouteRepository : GenericRepository<Route>, IRouteRepository
{
    private readonly IHolidayRepository _holidayRepository;

    public RouteRepository(DataContext dataContext, IHolidayRepository holidayRepository) : base(dataContext)
    {
        _holidayRepository = holidayRepository;
    }

    public async Task<ICollection<T>> GetRoutesByDateAsync<T>(Guid startStationId, Guid endStationId, DateTime date, Expression<Func<Route, T>> selector, CancellationToken cancellationToken = default)
    {
        IQueryable<Route> query = GetAll().AsNoTracking()
            .Include(route => route.StartStation)
            .Include(route => route.EndStation)
            .Where(route => route.StartStationId == startStationId && route.EndStationId == endStationId)
            .Where(route => route.TimeOfDeparture > date.TimeOfDay)
            .Where(route => route.Active);

        bool isHoliday = await _holidayRepository.IsHolidayAsync(date, cancellationToken);

        if (isHoliday)
        {
            query = query.Where(route => route.ActiveOnHolidays);
        }

        if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday) 
        {
            query = query.Where(route => route.ActiveOnWeekends);
        }

        return await query.OrderBy(route => route.TimeOfDeparture)
            .Select(selector)
            .ToListAsync(cancellationToken);
    }
}
