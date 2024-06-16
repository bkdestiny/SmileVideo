using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
namespace Common
{
    public class HttpHelper
    {
        public static string GetRemoteIpAddress(HttpContext httpContext)
        {
            StringValues remoteIpAddessString;
            httpContext.Request.Headers.TryGetValue("X-Real-IP", out remoteIpAddessString);
            string? remoteIpAddress= remoteIpAddessString.ToString();
            if(!string.IsNullOrWhiteSpace(remoteIpAddress))
            {
                return remoteIpAddress;
            }
            return httpContext.Connection.RemoteIpAddress.ToString();
        }


    }
}
