using Application.Services.Abstractions.Interfaces.Repositories.News;
using Infrastructure.Data;

namespace Infrastructure.Repositories.News
{
    public sealed class NewsRepository : GenericRepository<Domain.Entities.News.News>, INewsRepository
    {
        public NewsRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
