using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Result
    {
        public int Code {  get;private set; }

        public string Message { get;private set; }

        public object Data { get; private set; }


        private Result(int code, string message, object data)
        {
            Code = code;
            Message = message;
            Data = data;
        }

        public Result(int code, string message)
        {
            Code = code;
            Message = message;

        }
       
        public static Result Ok()
        {
            return new Result(200,"请求成功");
        }
        public static Result Ok(string message) {
            return new Result(200, message);
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
        public static Result Response(ResponseTypes type)
        {
            return Response(type, "");
        }
        public static Result Response(ResponseTypes type,object data)
        {
            FieldInfo? field = type.GetType().GetField(type.ToString());
            SimpleResponseAttribute[] attributes = (SimpleResponseAttribute[])field!.GetCustomAttributes(typeof(SimpleResponseAttribute), false);
            if (attributes == null || attributes.Length == 0)
            {
                throw new Exception("系统异常");
            }
            SimpleResponseAttribute resp = attributes[0];
            return new Result(resp.Code, resp.Message,data);
        }
    }
}
