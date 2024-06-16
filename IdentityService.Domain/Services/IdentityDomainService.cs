using Common.JWT;
using Common.Models;
using IdentityService.Domain.Entites;
using IdentityService.Domain.IRepositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace IdentityService.Domain.Services
{
    public class IdentityDomainService
    {
        private readonly IUserRepository userRepository;

        private readonly IRoleRepository roleRepository;

        private readonly UserManager<User> userManager;

        private readonly RoleManager<Role> roleManager;
        public IdentityDomainService(IUserRepository userRepository, IRoleRepository roleRepository, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<Result> InitAdmin(string userName,string password)
        {
            User? user=await userManager.FindByNameAsync(userName);
            if(user==null)
            {
                user = new User(userName);
                var ir=await userManager.CreateAsync(user,password);
                if(!ir.Succeeded)
                {
                    return Result.Error(500,"初始化失败");
                }
            }
            Role role = new Role("Admin");
            if (!await roleManager.RoleExistsAsync(role.Name!))
            {
                var ir=await roleManager.CreateAsync(role);
                if (!ir.Succeeded)
                {
                    return Result.Error(500, "初始化失败");
                }
            }
            if(!await userManager.IsInRoleAsync(user,role.Name))
            {
                var ir=await userManager.AddToRoleAsync(user, role.Name);
                if (!ir.Succeeded)
                {
                    return Result.Error(500, "初始化失败");
                }
            }
            return Result.Ok("初始化完成");
        }

        public async Task<User> LoginByUserNameAndPasswordAsync(string userName,string password)
        {
            User? user=await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return null!;
            }
            if (!await userManager.CheckPasswordAsync(user, password))
            {
                return null!;
            }
            return user;
        }
    }
}
