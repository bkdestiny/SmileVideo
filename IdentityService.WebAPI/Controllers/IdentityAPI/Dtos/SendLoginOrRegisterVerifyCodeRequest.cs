using FluentValidation;

namespace IdentityService.WebAPI.Controllers.IdentityAPI.Dtos
{
    public class SendLoginOrRegisterVerifyCodeRequest
    {
        public string PhoneNumber { get; set; }
    }

    public class SendLoginOrRegisterVerifyCodeRequestValidator : AbstractValidator<SendLoginOrRegisterVerifyCodeRequest>
    {
        public SendLoginOrRegisterVerifyCodeRequestValidator()
        {
            RuleFor(e => e.PhoneNumber).NotNull().NotEmpty().WithMessage("手机号码不能为空").Matches("^((13[0-9])|(14[5,7])|(15[0-3,5-9])|(17[0,3,5-8])|(18[0-9])|166|198|199|(147))\\d{8}$").WithMessage("手机号码格式不正确");
        }
    }

}
