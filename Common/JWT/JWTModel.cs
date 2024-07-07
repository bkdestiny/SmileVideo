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
    public record JWTModel(string name,string rule,string ipAddress,Guid avatar);
}
