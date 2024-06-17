using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Sms
{
    public interface ISms 
    {
        Task<(bool,string)> SendVerifyCodeAsync(string phoneNumber,string verifyCode);
    }
}
