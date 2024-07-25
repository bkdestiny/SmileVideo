using Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

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
            string errorMessage = "未知异常";
            if (ex is CommonException)
            {
                //自定义异常，直接返回异常信息
                CommonException? cex = ex as CommonException;
                context.Result = new ObjectResult(Result.Error(cex!.Code, cex.Message));
                errorMessage= cex.Message;
            }
            else
            {
                errorMessage = ex.Message;
                //其它异常
                if (he.IsDevelopment())
                {
                    //开发环境，直接返回异常信息
                    context.Result = new ObjectResult(Result.Error(500, ex.Message));
                }
                else
                {
                    //生产环境,隐藏异常信息
                    context.Result = new ObjectResult(Result.Error(500, "请求失败"));
                }
                
            }
            string errorTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            string realIpAddress = HttpHelper.GetRemoteIpAddress(context.HttpContext);
            //logger.LogError("IP:" + HttpHelper.GetRemoteIpAddress(context.HttpContext) +",Path:"+context.HttpContext.Request.Path+",Exception:{@ex}", new { source=ex.Source,message=ex.Message});
            logger.LogError("异常信息：{request},{errorInfo}", new { Ip = realIpAddress, Path = context.HttpContext.Request.Path }, new {Time=errorTime,Type=ex.GetType(),Source=ex.Source,Message= errorMessage });
            return Task.CompletedTask;
        }
    }
}
