using FluentValidation;

namespace IdentityService.WebAPI.Controllers.RoleAPI.Dtos
{
    public class GetRolePagingDataRequest 
    {
        public int PageSize { get; set; } = 0;

        public int PageIndex { get; set; } = 1;

        public string SearchText { get; set; } = "";

        public Guid UserId { get; set; } = Guid.Empty;
    }
    public class GetRolePagingDataRequestValidator : AbstractValidator<GetRolePagingDataRequest>
    {
        public GetRolePagingDataRequestValidator()
        {
            RuleFor(r => r.PageSize).Must(pageSize => pageSize >= 0 && pageSize <= 10000).WithMessage("请求参数[PageSize]错误");
            RuleFor(r => r.PageIndex).Must(pageIndex => pageIndex >= 1 && pageIndex <= 10000).WithMessage("请求参数[PageIndex]错误"); ;
        }
    }
}
