using Common;
using IdentityService.Domain.Entites;
using IdentityService.Domain.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<User> userManager;

        public UserRepository(UserManager<User> userManager)
        {
            this.userManager = userManager;
        }

        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            return userManager.CreateAsync(user, password); 
        }

    }
}
