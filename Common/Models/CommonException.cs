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
        /*        public int Code = 500;

                private string message = "请求失败";

                public int Code { get { return code; } }
                public string Message { get { return message; } }*/

        public int Code { get; set; } = 500;

        public string Message { get; set; } = "请求失败";

        public CommonException()
        {

        }

        public CommonException(int code)
        {
            this.Code = code;
        }
        public CommonException(string message)
        {
            this.Message = message;
        }
        public CommonException(int code, string message)
        {
            this.Code = code;
            this.Message = message; 
        }
    }
}
