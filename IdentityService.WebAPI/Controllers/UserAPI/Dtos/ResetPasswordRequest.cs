using FluentValidation;

namespace IdentityService.WebAPI.Controllers.UserAPI.Dtos
{
    public class ResetPasswordRequest
    {
        public Guid Id { get; set; } = Guid.Empty;

        public string? OldPassword { get; set; }
        public string NewPassword { get; set; }
        
        public string? VerifyCode { get; set; }
    }
    public class ResetPasswordFromAdminFromValidator : AbstractValidator<ResetPasswordRequest>
    {
        public ResetPasswordFromAdminFromValidator()
        {
            RuleFor(e=>e.Id).NotNull().NotEmpty().WithMessage("用户Id不能为空");
            RuleFor(e => e.NewPassword).NotNull().NotEmpty().WithMessage("新密码不能为空");
        }
    }
}
