using Application.Services.Abstractions.Interfaces.Repositories.Driver;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Driver.Delay
{
    public class DelayRepository : GenericRepository<Domain.Entities.Driver.Delay>, IDelayRepository
    {
        public DelayRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
