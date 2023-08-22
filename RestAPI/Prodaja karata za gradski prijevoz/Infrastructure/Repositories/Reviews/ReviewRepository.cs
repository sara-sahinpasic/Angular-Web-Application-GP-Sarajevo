using Application.Services.Abstractions.Interfaces.Repositories.Reviews;
using Domain.Entities.Reviews;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Reviews
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
