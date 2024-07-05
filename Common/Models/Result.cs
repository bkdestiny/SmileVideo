using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Common.Models
{
    public class Result
    {
        public int Code {  get;private set; }

        public string Message { get;private set; }

        public object? Data { get; private set; }

        /// <summary>
        /// 通用
        /// </summary>
        public static readonly Result UnknownError = new Result(520, "未知错误");
        public static readonly Result MissingRelevantConfiguration = new Result(500, "缺少相关配置");
        public static readonly Result TooManyRequests = new Result(429, "请求过于频繁,请稍后重试");
        public static readonly Result RequestParamError = new Result(400, "请求参数错误");
        public static readonly Result DataQueryFailed = new Result(500, "数据查询失败");

        /// <summary>
        /// IdentityService认证服务
        /// </summary>
        public static readonly Result UserNameOrPasswordError = new Result(500, "用户名或密码错误");
        public static readonly Result UserIsLocked = new Result(500, "用户已被锁定");

        /// <summary>
        /// FileService文件服务
        /// </summary>
        public static readonly Result ResourceNotExist = new Result(500, "资源不存在");


        private Result(int code, string message, object data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        private Result(int code, string message)
        {
            Code = code;
            Message = message;
        }
       
        public static Result Ok()
        {
            return new Result(200,"请求成功");
        }
        public static Result Ok(object data) {
            return new Result(200, "请求成功",data);
        }
        public static Result Ok(string message,object data) {
            return new Result(200, message, data);
        }
        public static Result Error()
        {
            return new Result(500, "请求失败");
        }
        public static Result Error(int code)
        {
            return new Result(code,"请求失败");
        }
        public static Result Error(string message) 
        {
            return new Result(500, message);
        }
        public static Result Error(int code,string message)
        {
            return new Result(code,message);
        }
    }
}
