using Common.JWT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class UserContext
    {
        private static readonly ThreadLocal<JWTModel> local = new ThreadLocal<JWTModel>();

        public static JWTModel UserInfo
        {
            get
            {
                if (local.Value == null)
                {
                    throw new Exception("在不需要JWT的接口中尝试获取用户信息");
                }
                return local.Value;
            }
            set { local.Value = value; }
        }
        public static void Remove()
        {
            if (local.Value != null)
            {
                local.Value.Dispose();
            }
        }
    }
}
