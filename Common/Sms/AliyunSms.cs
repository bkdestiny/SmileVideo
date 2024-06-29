using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlibabaCloud.OpenApiClient.Models;
using AlibabaCloud.SDK.Dysmsapi20170525;
using AlibabaCloud.SDK.Dysmsapi20170525.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
namespace Common.Sms
{
    public class AliyunSms : ISms
    {
        private readonly Client client;
        private readonly AliyunSmsOptions options;
        public AliyunSms(AliyunSmsOptions options)
        {
            this.options = options;
            Config config = new Config();
            config.RegionId = options.RegionId;
            config.AccessKeyId = options.AccessKey;
            config.AccessKeySecret = options.AccessKeySecret;
            config.SignatureVersion = options.SignatureVersion;
            this.client = new Client(config);
        }

        public SmsTemplate GetSmsTemplate()
        {
            return options.Templates;
        }

        public async Task<(bool, string)> SendAsync(string phoneNumbers, string templateCode,string templateParam, params string[] args)
        {
            SendSmsRequest request = new SendSmsRequest();
            request.SignName = options.SignName;
            request.TemplateCode = templateCode;
            request.PhoneNumbers = phoneNumbers;
            request.TemplateParam = "{" + string.Format(templateParam,args) + "}";
            try
            {
                SendSmsResponse response = await client.SendSmsAsync(request);
                if (response.Body.Code.Equals("OK"))
                {
                    return (true, response.Body.Message);
                }
                else
                {
                    return (false, response.Body.Message);
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
        }

        public async Task<(bool, string)> SendVerifyCodeAsync(string phoneNumber, string verifyCode)
        {
            SendSmsRequest request = new SendSmsRequest();
            request.SignName = options.SignName;
            request.TemplateCode = options.Templates.VerifyCode;
            request.PhoneNumbers = phoneNumber;
            request.TemplateParam = "{"+string.Format(options.Templates.VerifyCodeParam!, verifyCode)+"}";
            try
            {
                SendSmsResponse response = await client.SendSmsAsync(request);
                if (response.Body.Code.Equals("OK"))
                {
                    return (true, response.Body.Message);
                }
                else
                {
                    return (false, response.Body.Message);
                }
            }catch(Exception e)
            {
                return (false, e.Message);
            }

        }
    }
}
