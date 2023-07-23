using Domain.Entities.Requests;

namespace Application.Services.Abstractions.Interfaces.Repositories.Requests
{
    public interface IRequestRepository : IGenericRepository<Request>
    {
        Task<bool> HasAnyActiveRequests(Guid userId, CancellationToken cancellationToken = default);
    }
}
