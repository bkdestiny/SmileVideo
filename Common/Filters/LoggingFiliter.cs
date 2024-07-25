using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
namespace Common.Filters
{
    public class LoggingFiliter : IAsyncActionFilter
    {
        private readonly ILogger<LoggingFiliter> logger;

        public LoggingFiliter(ILogger<LoggingFiliter> logger)
        {
            this.logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if(context.ActionDescriptor is ControllerActionDescriptor)
            {
                ControllerActionDescriptor? ctd=context.ActionDescriptor as ControllerActionDescriptor;
                string realIpAddress = HttpHelper.GetRemoteIpAddress(context.HttpContext);
                string start= DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                ActionExecutedContext result = await next();
                object? resp="未知响应";
                if(result.Result is ObjectResult)
                {
                    resp = JsonSerializer.Serialize((result.Result as ObjectResult)!.Value);

                }
                string errorMessage = "未发生异常";
                if (result.Exception != null)
                {
                    if(result.Exception is CommonException)
                    {
                        errorMessage = (result.Exception as CommonException)!.Message;
                    }
                    else
                    {
                        errorMessage = result.Exception.Message;
                    }   
                }
                //logger.LogInformation("IP:" + ipAddress + ",Path:" + context.HttpContext.Request.Path + ",Request:{@req},Response:{@resp}", JsonSerializer.Serialize(context.ActionArguments), resp);
                stopwatch.Stop();
                double timeConsumptionSeconds = stopwatch.Elapsed.TotalSeconds;
                logger.LogInformation("请求信息：{info}", new { Ip = realIpAddress,UserInfo=JsonSerializer.Serialize(UserContext.UserInfo), Path = context.HttpContext.Request.Path,Authorization = JsonSerializer.Serialize(context.HttpContext.Request.Headers.SingleOrDefault(e=>e.Key.Equals("Authorization")).Value), Request = JsonSerializer.Serialize(context.ActionArguments), Response = resp, Error = errorMessage, StartTime = start, TimeConsumptionSeconds = timeConsumptionSeconds });
                return;
            }
            else
            {
                await next();
            }

            
        }
    }
}
