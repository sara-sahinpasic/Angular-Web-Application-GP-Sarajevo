using Application.Services.Abstractions.Interfaces.Repositories.Roles;
using Domain.Entities.Users;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Roles
{
    public sealed class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(DataContext dataContext) : base(dataContext)
        {
        }
    }
}
