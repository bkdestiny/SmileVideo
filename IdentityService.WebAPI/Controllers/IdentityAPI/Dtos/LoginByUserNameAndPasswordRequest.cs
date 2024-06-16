using FluentValidation;

namespace IdentityService.WebAPI.Controllers.IdentityAPI.Dtos
{
    public class LoginByUserNameAndPasswordRequest
    {

        public string UserName { get; set; }
        public string Password { get; set; }


    }

    public class LoginByUserNameAndPasswordRequestVaildator : AbstractValidator<LoginByUserNameAndPasswordRequest>
    {
        public LoginByUserNameAndPasswordRequestVaildator()
        {
            RuleFor(e => e.UserName).NotNull().NotEmpty().WithMessage("用户名或密码不能为空");
            RuleFor(e => e.Password).NotNull().NotEmpty().WithMessage("用户名或密码不能为空"); ;
        }
    }
}
