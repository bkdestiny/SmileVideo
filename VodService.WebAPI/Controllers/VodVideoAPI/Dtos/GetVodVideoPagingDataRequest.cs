using FluentValidation;

namespace VodService.WebAPI.Controllers.VodVideoAPI.Dtos
{
    public class GetVodVideoPagingDataRequest
    {
        public int PageSize { get; set; } = 0;

        public int PageIndex { get; set; } = 1;

        public Guid? ClassifyId { get; set; } = Guid.Empty;
    }
    public class GetVodVideoPagingDataRequestValidator : AbstractValidator<GetVodVideoPagingDataRequest>
    {
        public GetVodVideoPagingDataRequestValidator()
        {
            RuleFor(r => r.PageSize).Must(pageSize => pageSize >= 0 && pageSize <= 10000).WithMessage("请求参数[PageSize]错误");
            RuleFor(r => r.PageIndex).Must(pageIndex => pageIndex >= 1 && pageIndex <= 10000).WithMessage("请求参数[PageIndex]错误"); ;
        }
    }

}
