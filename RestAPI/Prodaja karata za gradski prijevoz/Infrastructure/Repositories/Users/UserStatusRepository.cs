using Application.Services.Abstractions.Interfaces.Repositories.Users;
using Domain.Entities.Users;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Users
{
    public class UserStatusRepository : GenericRepository<Status>, IUserStatusRepository
    {
        public UserStatusRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
