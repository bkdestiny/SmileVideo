using FluentValidation;
using VodService.Domain.Entities;

namespace VodService.WebAPI.Controllers.VodVideoClassifyAPI.Dtos
{
    public class VodVideoClassifyDto
    {
        public Guid Id { get; set; }

        public string ClassifyName { get; set; }

        public ClassifyTypes ClassifyType { get; set; } = ClassifyTypes.Type;

        public int SortIndex { get; set; }
    }
    public class VodVideoClassifyDtoValidator : AbstractValidator<VodVideoClassifyDto>
    {
        public VodVideoClassifyDtoValidator()
        {
            RuleFor(e => e.ClassifyName).NotNull().NotEmpty().WithMessage("分类名称不能空");
        }
    }
}
