using Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Common.Filters
{
    public class CommonExceptionFilter : IAsyncExceptionFilter
    {
        private IWebHostEnvironment he;
        private readonly ILogger<CommonExceptionFilter> logger;

        public CommonExceptionFilter(IWebHostEnvironment he, ILogger<CommonExceptionFilter> logger)
        {
            this.he = he;
            this.logger = logger;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;
            if (ex is CommonException)
            {
                CommonException? cex = ex as CommonException;
                context.Result = new ObjectResult(Result.Error(cex!.Code, cex.Message));
            }
            else
            {
                if (he.IsDevelopment())
                {
                    context.Result = new ObjectResult(Result.Error(500, ex.Message));
                }
                else
                {
                    context.Result = new ObjectResult(Result.Error(500, "请求失败"));
                }
                
            }
            logger.LogError("IP:" + HttpHelper.GetRemoteIpAddress(context.HttpContext) +",Path:"+context.HttpContext.Request.Path+",Exception:{@ex}", new { source=ex.Source,message=ex.Message});
            return Task.CompletedTask;
        }
    }
}
