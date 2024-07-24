using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.JWT
{
    /// <summary>
    /// JWT 可以被前端和用户获取的数据
    /// </summary>
    /// <param name="name">用户名</param>
    /// <param name="rule">用户角色,以逗号(,)分隔</param>
    /// <param name="ipAddress">Ip地址</param>
    /// <param name="avatar">头像FileId</param>
    public class JWTModel : IDisposable
    {
        private bool isDisposed = false;
        public Guid Id { get; init; }

        public string Name { get; init; }

        public string Role { get; init; }
        public string IpAddress { get; init; }

        public Guid Avatar {  get; init; }

        public JWTModel() { }

        public JWTModel(Guid id, string name, string role, string ipAddress, Guid avatar)
        {
            Id = id;
            Name = name;
            Role = role;
            IpAddress = ipAddress;
            Avatar = avatar;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    //...
                }

                // 释放非托管资源
                //...

                isDisposed = true;
            }
        }
    }
}
