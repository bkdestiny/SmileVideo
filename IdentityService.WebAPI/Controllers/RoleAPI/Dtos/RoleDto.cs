using FluentValidation;
using IdentityService.Domain.Entites;

namespace IdentityService.WebAPI.Controllers.RoleAPI.Dtos
{
    public class RoleDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; } = "";

        public RoleDto()
        {
        }

        public RoleDto(Role role)
        {
            Id = role.Id;
            Name = role.Name;
            Description = role.Description;
        }
    }

    public class AddRoleDto : AbstractValidator<RoleDto>
    {
        public AddRoleDto()
        {
            RuleFor(e => e.Name).NotNull().NotEmpty().WithMessage("角色名不能为空");
        }
    }
    public class UpdateRoleDto : AbstractValidator<RoleDto>
    {
        public UpdateRoleDto()
        {
            RuleFor(e => e.Id).NotNull().NotEmpty().WithMessage("Id不能为空");
            RuleFor(e => e.Name).NotNull().NotEmpty().WithMessage("角色名不能为空");
        }
    }

}
