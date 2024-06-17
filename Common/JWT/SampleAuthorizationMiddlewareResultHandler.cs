using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using System.Security.Claims;
namespace Common.JWT
{
    /// <summary>
    /// Microsoft文档 https://learn.microsoft.com/zh-cn/aspnet/core/security/authorization/customizingauthorizationmiddlewareresponse?view=aspnetcore-8.0
    /// </summary>
    public class SampleAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            string responseBody = "{\"code\":401,\"message\":\"没有权限\"}";
            bool access = false;
            try
            {
                if (context.User.Identity!.IsAuthenticated)
                {
                    long expirationTime = long.Parse(context.User.Claims.Single(c => c.Type.Equals("ExpirationTime")).Value);
                    string IpAddress = context.User.Claims.Single(c => c.Type.Equals("IpAddress")).Value;
                    if (new DateTime(expirationTime).CompareTo(DateTime.Now) == -1)
                    {
                        //Token已过期
                        responseBody = "{\"code\":403,\"message\":\"登录失效\"}";
                    }
                    else if (!HttpHelper.GetRemoteIpAddress(context).Equals(IpAddress))
                    {
                        //IP地址不合法
                        responseBody = "{\"code\":403,\"message\":\"登录失效\"}";
                    }
                    else
                    {
                        await next(context);
                        return;
                    }
                }
                else
                {
                    //Token为空或者校验不合法
                    responseBody = "{\"code\":403,\"message\":\"登录失效\"}";
                }
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync(responseBody);
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync(responseBody);
            }





        }
    }
}
