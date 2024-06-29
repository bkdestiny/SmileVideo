using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using Common.DistributeCache;
using Common.JWT;
using Common.Models;
using Common.Sms;
using IdentityService.Domain.Entites;
using IdentityService.Domain.DomainEvents;
using IdentityService.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace IdentityService.Domain.DomainServices
{
    public class IdentityDomainService
    {
        private readonly IUserRepository userRepository;

        private readonly IRoleRepository roleRepository;

        private readonly UserManager<User> userManager;

        private readonly RoleManager<Role> roleManager;

        private readonly IMediator mediator;

        private readonly IDistributeCacheService cache;

        private readonly ISms sms;

        private readonly ILogger<IdentityDomainService> logger;
        public IdentityDomainService(IUserRepository userRepository, IRoleRepository roleRepository, UserManager<User> userManager, RoleManager<Role> roleManager, IMediator mediator, ILogger<IdentityDomainService> logger, IDistributeCacheService cache, ISms sms)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mediator = mediator;
            this.logger = logger;
            this.cache = cache;
            this.sms = sms;
        }
        /// <summary>
        /// 初始化管理员
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 用户名和密码登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 登录或注册
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        /// <exception cref="CommonException"></exception>
        public async Task<User> LoginOrRegitserByPhoneNumberAndVerifyCodeAsync(string phoneNumber,string verifyCode)
        {
            string key = "LoginOrRegister:verifyCode:" + phoneNumber;
            string cacheCode=await cache.StringGetAsync(key);
            if (string.IsNullOrEmpty(cacheCode) ||!verifyCode.Equals(cacheCode))
            {
                throw new CommonException("验证码错误");
            }
            User? user=await userRepository.FindUserByPhoneNumberAsync(phoneNumber);
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
                user.PhoneNumber = phoneNumber;
                string password = CreatePassword();
                var ir=await userManager.CreateAsync(user, password);
                if (!ir.Succeeded)
                {
                    throw new CommonException("注册失败");
                }
                //注册成功 发布用户注册成功领域事件
                await mediator.Publish(new UserCreatedEvent(Guid.NewGuid(),userName,password,phoneNumber));
                return user;
            }         
        }
        /// <summary>
        /// 发送登录或注册短信验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        /// <exception cref="CommonException"></exception>
        public async Task<(bool,string?)> SendLoginOrRegisterVerifyCode(string phoneNumber)
        {
            string key = "LoginOrRegister:verifyCode:" + phoneNumber;
            //缓存是否存在
            string verifyCode= await cache.StringGetAsync(key);
            if (!string.IsNullOrEmpty(verifyCode))
            {
                return (false, "发送验证码频繁,请稍后重试");
            }
            //生成6位验证码
            verifyCode = new Random().Next(111111, 999999).ToString();
            //发送短信
            (bool smsSuccess,string smsMessage)=await sms.SendAsync(phoneNumber,sms.GetSmsTemplate().VerifyCode!,sms.GetSmsTemplate().VerifyCodeParam!,verifyCode);
            if (!smsSuccess)
            {
                logger.LogError("验证码短信发送失败：{info},错误信息："+smsMessage, new { phoneNumber = phoneNumber, verifyCode = verifyCode });
                return (false, "验证码短信发送失败");
            }
            //写入缓存
            await cache.SetStringAsync(key, verifyCode,60*5);
            return (true,null);
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
