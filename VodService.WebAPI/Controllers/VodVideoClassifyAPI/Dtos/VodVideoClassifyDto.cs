using FluentValidation;
using VodService.Domain.Entities;

namespace VodService.WebAPI.Controllers.VodVideoClassifyAPI.Dtos
{
    public class VodVideoClassifyDto
    {

        public Guid Id { get; set; } = Guid.Empty;

        public string ClassifyName { get; set; }

        public ClassifyTypes ClassifyType { get; set; } = ClassifyTypes.Type;

        public int SortIndex { get; set; }

        public VodVideoClassifyDto() { }
        public VodVideoClassifyDto(VodVideoClassify vodVideoClassify)
        {
            Id= vodVideoClassify.Id;
            ClassifyName= vodVideoClassify.ClassifyName;
            ClassifyType=vodVideoClassify.ClassifyType;
            SortIndex = vodVideoClassify.SortIndex;
        }

    }

    public class VodVideoClassifyDtoValidator : AbstractValidator<VodVideoClassifyDto>
    {
        public VodVideoClassifyDtoValidator()
        {
            RuleFor(e => e.ClassifyName).NotNull().NotEmpty().WithMessage("分类名称不能空");
        }
    }
}
