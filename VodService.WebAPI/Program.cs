
using Initializer;

namespace VodService.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region 通常服务注册
            builder.ConfigureExtraServices(new InitializerOptions
            {
                EventBusQueueName = "VodService.WebAPI",
                LogFilePath = $"d:/SmileVideo/VodService/logs/{DateTime.Now:yyyy-MM-dd}//logging.log"
            });
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
