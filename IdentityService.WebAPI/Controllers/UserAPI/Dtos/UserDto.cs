using Common.EFcore.Models;
using FluentValidation;
using IdentityService.Domain.Entites;
using Microsoft.AspNetCore.Identity;

namespace IdentityService.WebAPI.Controllers.UserAPI.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string UserName { get; set; }

        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Email { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        public bool IsLockout { get; set; } = false;

        public Guid Avatar {  get; set; }=Guid.Empty;


        public UserDto() { }
        public UserDto(User user)
        {
            this.Id = user.Id;
            this.UserName = user.UserName!;
            this.PhoneNumber = user.PhoneNumber;
            this.Email = user.Email;
            this.CreateTime = user.CreateTime;
            this.LockoutEnd = user.LockoutEnd != null ? DateTimeOffset.Parse(user.LockoutEnd.ToString()!).ToLocalTime() : null;
            if (this.LockoutEnd != null && this.LockoutEnd > DateTimeOffset.Now)
            {
                IsLockout = true;
            }
            this.Avatar=user.Avatar;
        }
    }
    public class AddUserDtoValidator : AbstractValidator<UserDto>
    {
        public AddUserDtoValidator()
        {
            RuleFor(e => e.UserName).NotEmpty().NotNull().Matches("^[a-zA-Z0-9_]{3,20}$").WithMessage("用户名长度为3至20个字符，数字、英文、下划线组成");
            RuleFor(e => e.PhoneNumber).Matches("^(?:(((13[0-9])|(14[5,7])|(15[0-3,5-9])|(17[0,3,5-8])|(18[0-9])|166|198|199|(147))\\d{8}))?$").WithMessage("手机号格式不正确");
            RuleFor(e => e.Email).Matches("^(?:[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,})$").WithMessage("电子邮箱格式不正确");
        }
    }
    public class UpdateUserDtoValidator : AbstractValidator<UserDto>
    {
        public UpdateUserDtoValidator()
        {
            RuleFor(e => e.UserName).NotEmpty().NotNull().Matches("^[a-zA-Z0-9_]{3,20}$").WithMessage("用户名长度为3至20个字符，[数字 英文 _]组成");
            RuleFor(e => e.PhoneNumber).Matches("^(?:(((13[0-9])|(14[5,7])|(15[0-3,5-9])|(17[0,3,5-8])|(18[0-9])|166|198|199|(147))\\d{8}))?$").WithMessage("手机号格式不正确");
            RuleFor(e => e.Email).Matches("^(?:[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,})$").WithMessage("电子邮箱格式不正确");
        }
    }
}
