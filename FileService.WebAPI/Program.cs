
using FileService.Domain.IStorage;
using FileService.Infrastructure.Storage;
using Initializer;

namespace FileService.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            IConfiguration configuration = builder.Configuration;

            #region 通常服务注册
            builder.ConfigureExtraServices(new InitializerOptions
            {
                EventBusQueueName = "FileService.WebAPI",
                LogFilePath = $"d:/SmileVideo/FileService/logs/{DateTime.Now:yyyy-MM-dd}//logging.log"
            });
            #endregion

            #region 腾讯云Cos
            TencentYunCosOptions tencentYunCosOptions= configuration.GetSection("TencentYunCos").Get<TencentYunCosOptions>();
            builder.Services.AddSingleton<IStorageService>(new TencentYunCosService(tencentYunCosOptions)); 
            #endregion

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
