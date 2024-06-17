using DotNetCore.CAP.Messages;
using Exceptionless.Models;
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
    public class ResultA : Dictionary<string, object>
    {
        private ResultA()
        {

        }
        public static ResultA Response(ResponseTypes type)
        {
            SimpleResponseAttribute resp = type.GetType().GetField(type.ToString()).GetCustomAttribute<SimpleResponseAttribute>();
            ResultA result = new ResultA();
            result.Add("code", resp.Code);
            result.Add("message", resp.Message);
            return result;
        }
        public static ResultA Response(ResponseTypes type, object data)
        {
            SimpleResponseAttribute resp = type.GetType().GetField(type.ToString()).GetCustomAttribute<SimpleResponseAttribute>();
            ResultA result = new ResultA();
            result.Add("code", resp.Code);
            result.Add("message", resp.Message);
            result.Add("data", data);
            return result;
        }
        public static ResultA Ok()
        {
            ResultA result = new ResultA();
            result.Add("code", 200);
            result.Add("message", "请求成功");
            result.Add("data", ""); ;
            return result;
        }
        public static ResultA Ok(string message,object data)
        {
            ResultA result = new ResultA();
            result.Add("code", 200);
            result.Add("message", message);
            result.Add("data", data); ;
            return result;
        }
        public static ResultA Ok(object data)
        {
            ResultA result = new ResultA();
            result.Add("code", 200);
            result.Add("message", "请求成功");
            result.Add("data", data); ;
            return result;
        }
        public static ResultA Error()
        {
            return Error(500);
        }
        public static ResultA Error(int code)
        {
            return Error(code, "请求失败");
        }
        public static ResultA Error(string message)
        {
            return Error(500, message);
        }
        public static ResultA Error(int code, string message)
        {
            ResultA result = new ResultA();
            result.Add("code", code);
            result.Add("message", message);
            return result;
        }
    }
}
