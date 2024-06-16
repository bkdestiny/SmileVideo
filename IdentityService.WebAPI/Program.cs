
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


            //������ʼ��
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
            //��¼��ע�����Ŀ����Ҫ����WebApplicationBuilderExtensions�еĳ�ʼ��֮�⣬��Ҫ���µĳ�ʼ��
            //��Ҫ��AddIdentity��������AddIdentityCore
            //��Ϊ��AddIdentity�ᵼ��JWT���Ʋ������ã�AddJwtBearer�лص����ᱻִ�У��������AuthenticationУ��ʧ��
            IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;//����
                options.Password.RequireLowercase = false;//Сд
                options.Password.RequireNonAlphanumeric = false;//����ĸ����
                options.Password.RequireUppercase = false;//��д
                options.Password.RequiredLength = 6;//����
                //�����趨RequireUniqueEmail��������������Ϊ��
                //options.User.RequireUniqueEmail = true;
                //�������У���GenerateEmailConfirmationTokenAsync��֤������
                options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultEmailProvider;
                options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;
            });
            identityBuilder = new IdentityBuilder(typeof(User), typeof(Role), builder.Services);
            //���Identity��ܺ�DBContext��ʵ����
            identityBuilder.AddEntityFrameworkStores<IdDbContext>()
                .AddDefaultTokenProviders()
                .AddRoleManager<RoleManager<Role>>()
                .AddUserManager<UserManager<User>>();

            //ע���йܷ���(��ʱ����)
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
