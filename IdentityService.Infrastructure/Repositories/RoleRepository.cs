using IdentityService.Domain.Entites;
using IdentityService.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IdDbContext idDbContext;

        public RoleRepository(IdDbContext idDbContext)
        {
            this.idDbContext = idDbContext;
        }

        public async Task DeleteRoleAsync(Guid id)
        {
            Role? role=await idDbContext.Roles.SingleOrDefaultAsync(e=>e.Id==id);
            if(role != null)
            {
                idDbContext.Roles.Remove(role);
            }
        }

        public async Task<IList<Role>> QueryRoleAsync(string searchText)
        {
            return await idDbContext.Roles.Where(e => !string.IsNullOrEmpty(searchText) ? e.Name.Contains(searchText) : true).ToListAsync();
        }
    }
}
