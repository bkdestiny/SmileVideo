
using Common;
using IdentityService.Domain.Entites;
using IdentityService.Infrastructure;
using IdentityService.WebAPI.BackgroundServices;
using Initializer;
using Microsoft.AspNetCore.Identity;


namespace IdentityService.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.


            //批量初始化
            builder.ConfigureExtraServices(new InitializerOptions
            {
                EventBusQueueName = "IdentityService.WebAPI",
                LogFilePath = $"d:/temp/{DateTime.Now:yyyy-MM-dd}//IdentityService.log"
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDataProtection();
            //登录、注册的项目除了要启用WebApplicationBuilderExtensions中的初始化之外，还要如下的初始化
            //不要用AddIdentity，而是用AddIdentityCore
            //因为用AddIdentity会导致JWT机制不起作用，AddJwtBearer中回调不会被执行，因此总是Authentication校验失败
            IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;//数字
                options.Password.RequireLowercase = false;//小写
                options.Password.RequireNonAlphanumeric = false;//非字母数字
                options.Password.RequireUppercase = false;//大写
                options.Password.RequiredLength = 6;//长度
                //不能设定RequireUniqueEmail，否则不允许邮箱为空
                //options.User.RequireUniqueEmail = true;
                //以下两行，把GenerateEmailConfirmationTokenAsync验证码缩短
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            });
            identityBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
            //结合Identity框架和DBContext的实体类
            identityBuilder.AddEntityFrameworkStores<IdDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<Role>>()
                .AddUserManager<UserManager<User>>();

            //注入托管服务(定时任务)
            //builder.Services.AddHostedService<HostedService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDefault();

            app.MapControllers();

            app.Run();
        }
    }
}
