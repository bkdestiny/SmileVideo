using FluentValidation;

namespace IdentityService.WebAPI.Controllers.UserAPI.Dtos
{
    public class ResetRolesOfUserReqeuest
    {
        public Guid UserId { get; set; } = Guid.Empty;

        public List<Guid> RoleIds { get; set; }=new List<Guid>();
    }
    public class ResetRolesOfUserRequestValidator : AbstractValidator<ResetRolesOfUserReqeuest>
    {
        public ResetRolesOfUserRequestValidator()
        {
            RuleFor(e => e.UserId).NotNull().NotEmpty().Must(userId=>userId!=Guid.Empty).NotEmpty().WithMessage("用户Id不能为空");
        }
    }
}
