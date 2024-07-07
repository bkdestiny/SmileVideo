using FluentValidation;

namespace VodService.WebAPI.Controllers.VodVideoAPI.Dtos
{
    public class VodVideoPagingDataRequest
    {
        public int PageSize { get; set; } = 10;

        public int PageIndex { get; set; } = 1;

        public Guid? ClassifyId { get; set; } = Guid.Empty;
    }
    public class VodVideoPagingDataRequestValidator : AbstractValidator<VodVideoPagingDataRequest>
    {
        public VodVideoPagingDataRequestValidator()
        {
            RuleFor(r => r.PageSize).Must(pageSize => pageSize >= 1 && pageSize <= 10000).WithMessage("请求参数[PageSize]错误");
            RuleFor(r => r.PageIndex).Must(pageIndex => pageIndex >= 1 && pageIndex <= 10000).WithMessage("请求参数[PageIndex]错误"); ;
        }
    }

}
