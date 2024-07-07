using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Sms
{
    /// <summary>
    /// 在appsettings.json的AliyunSms的Templates配置以下参数
    /// 模板: "Templates": {
    ///         "VerifyCode": "SMS_264790013",
    ///         "VerifyCodeParam": "\"code\":\"{0}\""
    ///        }
    /// </summary>
    public class SmsTemplate
{
    public string? VerifyCode { get; set; } //验证码模板

    public string? VerifyCodeParam { get; set; } //验证码参数模板

    public string? UserPassword { get; set; } //通知用户密码 模板

    public string? UserPasswordParam { get; set; } //通知用户密码 参数模板

}
}
