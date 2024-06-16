using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class CommonException : Exception
    {
        private int code = 500;

        private string message = "请求失败";

        public int Code { get { return code; } }
        public string Message { get { return message; } }

        public CommonException()
        {

        }
        public CommonException(ResponseTypes type)
        {
            try
            {
                SimpleResponseAttribute? rep = type.GetType().GetField(type.ToString()).GetCustomAttribute<SimpleResponseAttribute>();
                code = rep.Code;
                message = rep.Message;
            }
            catch (Exception ex)
            {

            }
        }

        public CommonException(int code)
        {
            this.code = code;
        }
        public CommonException(string message)
        {
            this.message = message;
        }
        public CommonException(int code, string message)
        {
            this.code = code;
            this.message = message;
        }
    }
}
