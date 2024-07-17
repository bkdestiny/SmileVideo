using Common.Attributes;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Common.Filters
{
    public class UnitOfWorkFilter : IAsyncActionFilter
    {
        private static UnitOfWorkAttribute? GetUoWAttr(ActionDescriptor actionDesc)
        {
            var caDesc = actionDesc as ControllerActionDescriptor;
            if (caDesc == null)
            {
                return null;
            }
            //try to get UnitOfWorkAttribute from controller,
            //if there is no UnitOfWorkAttribute on controller, 
            //try to get UnitOfWorkAttribute from action
            var uowAttr = caDesc.MethodInfo
                    .GetCustomAttribute<UnitOfWorkAttribute>();
            if (uowAttr != null)
            {
                return uowAttr;
            }
            else
            {
                return caDesc.ControllerTypeInfo
                .GetCustomAttribute<UnitOfWorkAttribute>(); 
            }
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var uowAttr = GetUoWAttr(context.ActionDescriptor);
            if (uowAttr == null)
            {
                await next();
                return;
            }
            if (uowAttr.EnableTransaction) {
                //开启事务
                using TransactionScope txScope = new(TransactionScopeAsyncFlowOption.Enabled);
                List<DbContext> dbCtxs = new List<DbContext>();
                foreach (var dbCtxType in uowAttr.DbContextTypes)
                {
                    //用HttpContext的RequestServices
                    //确保获取的是和请求相关的Scope实例
                    var sp = context.HttpContext.RequestServices;
                    DbContext dbCtx = (DbContext)sp.GetRequiredService(dbCtxType);
                    //dbCtx.Database.SetCommandTimeout(TimeSpan.FromSeconds(uowAttr.CommandTimeOut));
                    dbCtxs.Add(dbCtx);
                }
                var result = await next();
                if (result.Exception == null)
                {
                    foreach (var dbCtx in dbCtxs)
                    {
                        await dbCtx.SaveChangesAsync();
                    }
                    txScope.Complete();
                }
            }
            else
            {
                //不开启事务
                List<DbContext> dbCtxs = new List<DbContext>();
                foreach (var dbCtxType in uowAttr.DbContextTypes)
                {
                    //用HttpContext的RequestServices
                    //确保获取的是和请求相关的Scope实例
                    var sp = context.HttpContext.RequestServices;
                    DbContext dbCtx = (DbContext)sp.GetRequiredService(dbCtxType);
                    //dbCtx.Database.SetCommandTimeout(TimeSpan.FromSeconds(uowAttr.CommandTimeOut));
                    dbCtxs.Add(dbCtx);
                }
                var result = await next();
                if (result.Exception == null)
                {
                    foreach (var dbCtx in dbCtxs)
                    {
                        await dbCtx.SaveChangesAsync();
                    }
                }
            }
            
        }
    }
}
