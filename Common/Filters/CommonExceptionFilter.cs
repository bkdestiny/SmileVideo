using Common.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace Common.Filters
{
    public class CommonExceptionFilter : IAsyncExceptionFilter
    {
        private IWebHostEnvironment he;

        public CommonExceptionFilter(IWebHostEnvironment he)
        {
            this.he = he;
        }

        public Task OnExceptionAsync(ExceptionContext context)
        {
            var ex = context.Exception;
            if (ex is CommonException)
            {
                CommonException cex = ex as CommonException;
                context.Result = new ObjectResult(Result.Error(cex.Code, cex.Message));
                return Task.CompletedTask;
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

                return Task.CompletedTask;
            }
        }
    }
}
