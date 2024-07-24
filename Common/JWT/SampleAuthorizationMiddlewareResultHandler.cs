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
using System.Net;
using Microsoft.AspNetCore.Authorization.Infrastructure;
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
                string realIpAddress = HttpHelper.GetRemoteIpAddress(context);
                //if (context.User.Identity!.IsAuthenticated)
                if(authorizeResult.Succeeded)
                {
                    //JWT初步校验通过(角色和Token合法性）
                    string jwtIpAddress = context.User.Claims.Single(c => c.Type.Equals("IpAddress")).Value;
                    long expirationTime = long.Parse(context.User.Claims.Single(c => c.Type.Equals("ExpirationTime")).Value);
                    if (new DateTime(expirationTime)<=DateTime.Now)
                    {
                        //Token已过期
                        responseBody = "{\"code\":403,\"message\":\"登录失效\"}";
                        Console.WriteLine(realIpAddress + "携带的token已过期");
                    }
                    else if (!realIpAddress.Equals(jwtIpAddress))
                    {
                        //IP地址不合法
                        responseBody = "{\"code\":403,\"message\":\"登录失效\"}";
                        Console.WriteLine(realIpAddress + "携带的token与Ip地址不一致");
                    }
                    else
                    {
                        //校验完全通过
                        JWTModel userInfo=JWTHelper.BuildJWTModel(context.User.Claims);
                        UserContext.UserInfo = userInfo;
                        await next(context);
                        UserContext.Remove();
                        return;
                    }
                }
                else
                {
                    if(authorizeResult.AuthorizationFailure!=null&&authorizeResult.AuthorizationFailure.FailedRequirements.Any(a=>a is RolesAuthorizationRequirement))
                    {
                        //没有权限
                        responseBody = "{\"code\":401,\"message\":\"没有权限\"}";
                    }
                    else
                    {
                        //Token为空或者校验不合法
                        responseBody = "{\"code\":403,\"message\":\"登录失效\"}";
                    }
                    Console.WriteLine(realIpAddress + "未携带token或校验不合法");
                }
                context.Response.StatusCode = 200;
                await context.Response.WriteAsync(responseBody);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
