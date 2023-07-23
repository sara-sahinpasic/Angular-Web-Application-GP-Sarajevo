using Application.Services.Abstractions.Interfaces.Repositories.Requests;
using Domain.Entities.Requests;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Requests
{
    public class RequestRepository : GenericRepository<Request>, IRequestRepository
    {
        public RequestRepository(DataContext dataContext, IRequestRepository requestRepository) : base(dataContext)
        {
        }

        public Task<bool> HasAnyActiveRequests(Guid userId, CancellationToken cancellationToken = default)
        {
            return GetAll()
                .Where(request => request.UserId == userId && request.Active)
                .AnyAsync(cancellationToken);
        }
    }
}
