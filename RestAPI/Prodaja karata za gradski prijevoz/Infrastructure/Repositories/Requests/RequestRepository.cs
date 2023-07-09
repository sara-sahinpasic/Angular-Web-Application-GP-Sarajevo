using Application.Services.Abstractions.Interfaces.Repositories.Requests;
using Domain.Entities.Requests;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Requests
{
    public class RequestRepository : GenericRepository<Request>, IRequestRepository
    {
        public RequestRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
