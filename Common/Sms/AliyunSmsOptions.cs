using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Sms
{
    public record AliyunSmsOptions(string RegionId,string SignName,AliyunSmsTemplate Templates,string AccessKey,string AccessKeySecret,string SignatureVersion);

    
}
