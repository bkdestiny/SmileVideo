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

        Task<User?> FindUserByPhoneNumberAsync(string phoneNumber);

        Task<IList<User>> QueryUserAsync(string searchText);

        Task DeleteUserByIdAsync(Guid id);

        Task<User?> SelectUserByIdAsync(Guid id);

        void UpdateUser(User user);
    }
}
