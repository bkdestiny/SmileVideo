using Common.Attributes;
using Common.DistributeCache;
using Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Filters
{
    public class IdempotentFilter : IAsyncActionFilter
    {
        private readonly IDistributeCacheService cache;

        public IdempotentFilter(IDistributeCacheService cache)
        {
            this.cache = cache;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                ControllerActionDescriptor? cad = context.ActionDescriptor as ControllerActionDescriptor;
                if (cad!.MethodInfo.IsDefined(typeof(IdempotentAttribute), false))
                {
                    string ipAddress = HttpHelper.GetRemoteIpAddress(context.HttpContext);
                    string key = "lock:idempotent:" + ipAddress + ":" + context.HttpContext.Request.Path;
                    string token = DateTime.Now.Ticks.ToString();
                    if (await cache.tryLockAsync(key, token, 5, 0))
                    {
                        try
                        {
                            await next();
                        }
                        finally
                        {
                            await cache.unLockAsync(key, token);
                        }
                        return;
                    }
                    else
                    {
                        context.Result = new ObjectResult(Result.Error("请勿重复请求"));
                        return;
                    }
                }
            }
            await next();
        }
    }
}
