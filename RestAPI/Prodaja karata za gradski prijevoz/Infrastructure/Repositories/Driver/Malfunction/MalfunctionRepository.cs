using Application.Services.Abstractions.Interfaces.Repositories.Driver;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Driver.Malfunction
{
    public class MalfunctionRepository : GenericRepository<Domain.Entities.Driver.Malfunction>, IMalfunctionRepository
    {
        public MalfunctionRepository(DataContext dataContext) : base(dataContext)
        {

        }
    }
}
