using FluentValidation;

namespace VodService.WebAPI.Controllers.VodVideoPartAPI.Dtos
{
    public class GetVodVideoPartPagingDataRequest
    {
        public int PageSize { get; set; } = 0;

        public int PageIndex { get; set; } = 1;

        public Guid VideoId { get; set; }
    }
    public class GetVodVideoPartPagingDataRequestValidator : AbstractValidator<GetVodVideoPartPagingDataRequest>
    {
        public GetVodVideoPartPagingDataRequestValidator()
        {
            RuleFor(r => r.PageSize).Must(pageSize => pageSize >= 0 && pageSize <= 10000).WithMessage("请求参数[PageSize]错误");
            RuleFor(r => r.PageIndex).Must(pageIndex => pageIndex >= 1 && pageIndex <= 10000).WithMessage("请求参数[PageIndex]错误"); ;
            RuleFor(r => r.VideoId).NotNull().NotEmpty().Must(videoId=>videoId!=Guid.Empty).WithMessage("请求参数错误");
        }
    }
}
