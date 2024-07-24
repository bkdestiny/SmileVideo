using Common.EFcore.Models;
using FluentValidation;
using Microsoft.OpenApi.Writers;

namespace VodService.WebAPI.Controllers.VodVideoCommentAPI.Dtos
{
    public class GetVodVideoCommentPagingDataRequest
    {
        public int PageSize { get; set; } = 0;

        public int PageIndex { get; set; } = 1;

        public Guid RootVideoCommentId { get; set; } = Guid.Empty;

        public SortOrders? SortOrderOfCreateTime { get; set; }
        public Guid VideoId { get; set; }
    }

    public class GetVodVideoCommentPagingDataRequestValidator : AbstractValidator<GetVodVideoCommentPagingDataRequest>
    {
        public GetVodVideoCommentPagingDataRequestValidator()
        {
            RuleFor(r => r.PageSize).Must(pageSize => pageSize >= 0 && pageSize <= 10000).WithMessage("请求参数[PageSize]错误");
            RuleFor(r => r.PageIndex).Must(pageIndex => pageIndex >= 1 && pageIndex <= 10000).WithMessage("请求参数[PageIndex]错误"); ;
            RuleFor(r => r.VideoId).NotNull().NotEmpty().Must(videoId => videoId != Guid.Empty).WithMessage("参数错误");
        }
    }
}
