using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public enum ResponseTypes
    {
        /// <summary>
        /// 通用
        /// </summary>
        [SimpleResponse(200, "请求成功")]
        Success,
        [SimpleResponse(500, "请求失败")]
        Error,
        [SimpleResponse(520, "未知错误")]
        UnknownError,
        [SimpleResponse(400, "请求参数错误")]
        RequestParamError,
        [SimpleResponse(500, "缺少相关配置")]
        MissingRelevantConfiguration,
        [SimpleResponse(429, "请求过于频繁,请稍后重试")]
        TooManyRequests,
        /// <summary>
        /// Identity认证服务
        /// </summary>
        [SimpleResponse(500, "用户名或密码错误")]
        UserNameOrPasswordError,
        [SimpleResponse(500, "用户已被锁定")]
        UserIsLocked

    }
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class SimpleResponseAttribute : Attribute
    {
        public SimpleResponseAttribute(int code, string message)
        {
            Code = code;
            Message = message;
        }

        public int Code { get; private set; }
        public string Message { get; private set; }


    }
}
