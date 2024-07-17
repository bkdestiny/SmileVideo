using Common.EFcore.Models;
using FluentValidation;
using VodService.Domain.Entities;

namespace VodService.WebAPI.Controllers.VodVideoClassifyAPI.Dtos
{
    public class GetVodVideoClassifyPagingDataRequest
    {
        public int PageSize { get; set; } = 0;

        public int PageIndex { get; set; } = 1;

        public string? SearchText { get; set; }

        public SortOrders? SortOrderOfSortIndex {  get; set; }
        public ClassifyTypes? classifyType {  get; set; }
    }
    public class GetVodVideoClassifyPagingDataRequestValidator : AbstractValidator<GetVodVideoClassifyPagingDataRequest>
    {
        public GetVodVideoClassifyPagingDataRequestValidator()
        {
            RuleFor(r => r.PageSize).Must(pageSize => pageSize >= 0 && pageSize <= 10000).WithMessage("请求参数[PageSize]错误");
            RuleFor(r => r.PageIndex).Must(pageIndex => pageIndex >= 1 && pageIndex <= 10000).WithMessage("请求参数[PageIndex]错误"); ;
        }
    }
}
