

namespace IdentityService.WebAPI.BackgroundServices
{
    public class HostedService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Console.WriteLine("托管服务启动");
                while (!stoppingToken.IsCancellationRequested)
                {
                    Console.WriteLine("执行:" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"));
                    await Task.Delay(3000);
                }    
                
            }
            catch (Exception ex)
            {

            }
        }
    }
}
