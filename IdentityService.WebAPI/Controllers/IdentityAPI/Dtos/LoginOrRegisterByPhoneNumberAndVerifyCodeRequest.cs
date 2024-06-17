using FluentValidation;

namespace IdentityService.WebAPI.Controllers.IdentityAPI.Dtos
{
    public class LoginOrRegisterByPhoneNumberAndVerifyCodeRequest
    {
        public string PhoneNumber { set; get; }

        public string VerifyCode { set; get; }
    }

    public class LoginOrRegisterByPhoneNumberAndVerifyCodeRequestValidator : AbstractValidator<LoginOrRegisterByPhoneNumberAndVerifyCodeRequest>
    {
        public LoginOrRegisterByPhoneNumberAndVerifyCodeRequestValidator()
        {
            RuleFor(e => e.PhoneNumber).NotNull().NotEmpty().WithMessage("请输入手机号码").Matches(@"^(\\+86\\-)1[345789]\\d{9}$").WithMessage("手机号码格式不正确");
            RuleFor(e => e.VerifyCode).NotNull().NotEmpty().WithMessage("请输入验证码").Matches(@"^\d{6}$").WithMessage("验证码格式不正确");
        }
    }
}
