using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
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
            if (authorizeResult.Forbidden) {
                await context.Response.WriteAsync(Result.Error().ToString());
            }
            await defaultHandler.HandleAsync(next,context, policy, authorizeResult);
        }
    }
}
