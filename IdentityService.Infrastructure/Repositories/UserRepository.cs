using Common;
using IdentityService.Domain.Entites;
using IdentityService.Domain.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

        private IdDbContext idDbContext;

        public UserRepository(UserManager<User> userManager, IdDbContext idDbContext)
        {
            this.userManager = userManager;
            this.idDbContext = idDbContext;
        }

        public Task<IdentityResult> CreateAsync(User user, string password)
        {
            return userManager.CreateAsync(user, password); 
        }

        public async Task DeleteUserByIdAsync(Guid id)
        {
            User? user=await idDbContext.Users.SingleOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                idDbContext.Remove(user);
            }
        }

        public async Task<User?> FindUserByPhoneNumberAsync(string phoneNumber)
        {
            //return idDbContext.Users.Where(u => u.PhoneNumberConfirmed&&u.PhoneNumber==phoneNumber).First();
            return await idDbContext.Users.Where(u => u.PhoneNumber == phoneNumber).FirstOrDefaultAsync<User>();
        }
        

        public async Task<IList<User>> QueryUserAsync(string searchText)
        {
            return await idDbContext.Users.Where(e => !string.IsNullOrEmpty(searchText) ? e.UserName.Contains(searchText) : true).ToListAsync();
        }

        public async Task<User?> SelectUserByIdAsync(Guid id)
        {
            return await idDbContext.Users.SingleOrDefaultAsync(e=>e.Id == id);
        }

        public void UpdateUser(User user)
        {
            idDbContext.Users.Update(user);
        }
    }
}
