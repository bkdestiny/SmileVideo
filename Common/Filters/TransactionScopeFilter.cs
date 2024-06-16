using Common.Attributes;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Transactions;

namespace Common.Filters
{
    public class TransactionScopeFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor)
            {
                ControllerActionDescriptor? ad = context.ActionDescriptor as ControllerActionDescriptor;
                if (!ad!.MethodInfo.IsDefined(typeof(NoTransactionalAttribute), true))
                {
                    using (var taScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                    {
                        var result = await next();
                        if (result.Exception == null)
                        {
                            taScope.Complete();
                        }
                    }
                }
            }
        }
    }
}
