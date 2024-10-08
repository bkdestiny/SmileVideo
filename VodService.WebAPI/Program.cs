
using Initializer;
using Microsoft.Extensions.Configuration;
using VodService.Domain.DomainServices;
using VodService.Domain.IRepositories;
using VodService.Infrastructure.Repositories;

namespace VodService.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            IConfiguration configuration = builder.Configuration;
            // Add services to the container.
            #region 通常服务注册
            string? logRootDir = configuration.GetValue<string>("LogRootDir");
            if (logRootDir == null)
            {
                logRootDir = "d://SmileVideo//VodService//logs";
            }
            builder.ConfigureExtraServices(new InitializerOptions
            {
                ApplicationName="VodService",
                EventBusQueueName = "VodService.WebAPI",
                LogFilePath = $"{logRootDir}/{DateTime.Now:yyyy-MM-dd}//logging.log"
            });
            #endregion

            builder.Services.AddScoped<IVodVideoRepository, VodVideoRepository>();
            builder.Services.AddScoped<IVodVideoClassifyRepository, VodVideoClassifyRepository>();
            builder.Services.AddScoped<IVodVideoPartRepository, VodVideoPartRepository>();
            builder.Services.AddScoped<IVodVideoCommentRepository, VodVideoCommentRepository>();
            builder.Services.AddScoped<VodDomainService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            
            #region 通常
            app.UseDefault();
            #endregion
            
            app.MapControllers();

            app.Run();
        }
    }
}
