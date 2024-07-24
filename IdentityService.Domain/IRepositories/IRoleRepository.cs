using IdentityService.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain.IRepositories
{
    public interface IRoleRepository
    {
        Task<IList<Role>> QueryRoleAsync(string searchText);

        Task DeleteRoleAsync(Guid id);
    }
}
