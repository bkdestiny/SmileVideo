
using FileService.Domain.DomainServices;
using FileService.Domain.IRepositories;
using FileService.Domain.IStorage;
using FileService.Infrastructure.Repositories;
using FileService.Infrastructure.Storage;
using Initializer;
using Microsoft.AspNetCore.Http.Features;

namespace FileService.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            IConfiguration configuration = builder.Configuration;


            builder.Services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 3_000_000_000; // 设置文件上传大小限制为 3 GB
            });

            string? logRootDir = configuration.GetValue<string>("LogRootDir");
            if(logRootDir == null)
            {
                logRootDir = "d://SmileVideo//FileService//logs";
            }
            #region 通常服务注册
            builder.ConfigureExtraServices(new InitializerOptions
            {
                ApplicationName = "FileService",
                EventBusQueueName = "FileService.WebAPI",
                LogFilePath = $"{logRootDir}/{DateTime.Now:yyyy-MM-dd}//logging.log"
            });
            #endregion

            builder.Services.AddScoped<ISysFileRepository, SysFileRepository>();
            builder.Services.AddScoped<SysFileDomainService>();

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
