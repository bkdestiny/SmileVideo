using Common.DistributeCache;
using Common.JWT;
using Common.Models;
using IdentityService.Domain.Entites;
using IdentityService.Domain.IRepositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.ObjectPool;
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


        private readonly IDistributeCacheService cache;
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
        public async Task<User> LoginOrRegitserByPhoneNumberAndVerifyCodeAsync(string phoneNumber,string verifyCode)
        {
            string key = "LoginOrRegister:verifyCode:" + phoneNumber;
            string cacheCode=await cache.StringGetAsync(key);
            if (string.IsNullOrEmpty(cacheCode) ||!verifyCode.Equals(cacheCode))
            {
                throw new CommonException("验证码错误");
            }
            User? user=userRepository.FindUserByPhoneNumber(phoneNumber);
            if (user != null)
            {
                //手机号码存在 登录
                return user;
            }
            else
            {
                //手机号码不存在 注册
                string userName =await CreateUserNameAsync();
                user = new User(userName);
                string password = CreatePassword();
                var ir=await userManager.CreateAsync(user, password);
                if (!ir.Succeeded)
                {
                    throw new CommonException("注册失败");
                }
                //发送短信通知用户密码
                return user;
            }         
        }

        private async Task<string> CreateUserNameAsync()
        {
            try
            {
                string userName = "";
                User? user;
                do
                {
                    userName = "sv_" + new Random().Next(1000000000,1999999999);
                    user = await userManager.FindByNameAsync(userName);
                } while (user!=null);
                return userName;

            }catch(Exception e)
            {
                return "";
            }
        }
        private string CreatePassword()
        {
            return new Random().Next(10000000, 99999999).ToString();
        }
    }
}
