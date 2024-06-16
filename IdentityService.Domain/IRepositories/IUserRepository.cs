using IdentityService.Domain.Entites;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Domain.IRepositories
{
    public interface IUserRepository
    {
        Task<IdentityResult> CreateAsync(User user,string password);
    }
}
