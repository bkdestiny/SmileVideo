using FluentValidation;

namespace FileService.WebAPI.Controllers.SysFileAPI.Dtos
{
    public class GetSysFilePagingDataRequest
    {
        public int PageSize { get; set; } = 0;

        public int PageIndex { get; set; } = 1;

        public string FileIds { get; set; } = "";

    }
    public class GetSysFilePagingDataRequestValidator : AbstractValidator<GetSysFilePagingDataRequest>
    {
        public GetSysFilePagingDataRequestValidator()
        {
            RuleFor(r => r.PageSize).Must(pageSize => pageSize >= 0 && pageSize <= 10000).WithMessage("请求参数[PageSize]错误");
            RuleFor(r => r.PageIndex).Must(pageIndex => pageIndex >= 1 && pageIndex <= 10000).WithMessage("请求参数[PageIndex]错误"); ;
        }
    }

}
