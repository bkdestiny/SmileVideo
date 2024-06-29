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

        Task<(bool, string)> SendAsync(string phoneNumbers, string templateCode,string templateParam, params string[] args);

        SmsTemplate GetSmsTemplate();
    }
}
