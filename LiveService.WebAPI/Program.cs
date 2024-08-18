
using Initializer;
using LiveService.Domain.DomainServices;
using LiveService.Domain.IRepositories;
using LiveService.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace LiveService.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            #region 通常服务注册
            string? logRootDir = builder.Configuration.GetValue<string>("LogRootDir");
            if (logRootDir == null)
            {
                logRootDir = "d://SmileVideo//LiveService//logs";
            }
            builder.ConfigureExtraServices(new InitializerOptions
            {
                ApplicationName = "LiveService",
                EventBusQueueName = "LiveService.WebAPI",
                LogFilePath = $"{logRootDir}/{DateTime.Now:yyyy-MM-dd}//logging.log"
            });
            #endregion

            builder.Services.AddScoped<LiveDomainService>();
            builder.Services.AddScoped<ILiveRoomRepository, LiveRoomRepository>();

            // Add services to the container.

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
