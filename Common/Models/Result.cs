using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Common.Models
{
    public class Result : Dictionary<string, object>
    {
        private Result()
        {

        }
        public static Result Response(ResponseTypes type)
        {
            SimpleResponseAttribute resp = type.GetType().GetField(type.ToString()).GetCustomAttribute<SimpleResponseAttribute>();
            Result result = new Result();
            result.Add("code", resp.Code);
            result.Add("message", resp.Message);
            return result;
        }
        public static Result Response(ResponseTypes type, object data)
        {
            SimpleResponseAttribute resp = type.GetType().GetField(type.ToString()).GetCustomAttribute<SimpleResponseAttribute>();
            Result result = new Result();
            result.Add("code", resp.Code);
            result.Add("message", resp.Message);
            result.Add("data", data);
            return result;
        }
        public static Result Ok()
        {
            Result result = new Result();
            result.Add("code", 200);
            return result;
        }
        public static Result Ok(object data)
        {
            Result result = new Result();
            result.Add("code", 200);
            result.Add("data", data);
            return result;
        }
        public static Result Error()
        {
            return Error(500);
        }
        public static Result Error(int code)
        {
            return Error(code, "请求失败");
        }
        public static Result Error(string message)
        {
            return Error(500, message);
        }
        public static Result Error(int code, string message)
        {
            Result result = new Result();
            result.Add("code", code);
            result.Add("message", message);
            return result;
        }
    }
}
