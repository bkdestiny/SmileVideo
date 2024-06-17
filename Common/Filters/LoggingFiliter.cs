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
                string ipAddress = HttpHelper.GetRemoteIpAddress(context.HttpContext);
                ActionExecutedContext result = await next();
                object? resp="未知响应";
                if (result.Exception == null)
                {
                     resp = (result.Result as ObjectResult)!.Value;
                }
                else
                {
                    resp = result.Exception.Message;
                }
                logger.LogInformation("IP:" + ipAddress + ",Path:" + context.HttpContext.Request.Path + ",Request:{@req},Response:{@resp}", JsonSerializer.Serialize(context.ActionArguments), resp);
                return;
            }
            else
            {
                await next();
            }

            
        }
    }
}
