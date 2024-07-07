using FluentValidation;
using VodService.Domain.Entities;

namespace VodService.WebAPI.Controllers.VodVideoClassifyAPI.Dtos
{
    public class VodVideoClassifyPagingDataRequest
    {
        public int PageSize { get; set; } = 10;

        public int PageIndex { get; set; } = 1;

        public ClassifyTypes? classifyType {  get; set; }
    }
    public class VodVideoClassifyPagingDataRequestValidator : AbstractValidator<VodVideoClassifyPagingDataRequest>
    {
        public VodVideoClassifyPagingDataRequestValidator()
        {
            RuleFor(r => r.PageSize).Must(pageSize => pageSize >= 1 && pageSize <= 10000).WithMessage("请求参数[PageSize]错误");
            RuleFor(r => r.PageIndex).Must(pageIndex => pageIndex >= 1 && pageIndex <= 10000).WithMessage("请求参数[PageIndex]错误"); ;
        }
    }
}
