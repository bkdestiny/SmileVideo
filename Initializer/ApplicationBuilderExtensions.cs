using AspNetCoreRateLimit;
using Common.Models;
using Microsoft.AspNetCore.Builder;

namespace Initializer
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseDefault(this IApplicationBuilder app)
        {
            //app.UseEventBus();
            app.UseIpRateLimiting();//启用客户端IP限制速率
            //app.UseClientRateLimiting();//启用客户端ID限制速率
            app.UseCors();//启用Cors 跨域问题
            app.UseForwardedHeaders();
            //app.UseHttpsRedirection();//启用Https重定向 不能与ForwardedHeaders很好的工作，而且webapi项目也没必要配置这个
            app.UseAuthentication();
            app.UseAuthorization();
            return app;
        }
    }
}
