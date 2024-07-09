using FluentValidation;
using VodService.Domain.Entities;
using VodService.WebAPI.Controllers.VodVideoAPI.Dtos;
using static VodService.Domain.Entities.VodVideoPart;

namespace VodService.WebAPI.Controllers.VodVideoPartAPI.Dtos
{
    public class VodVideoPartDto
    {

        public Guid Id { get; set; } = Guid.Empty;
        public string PartName { get; set; }
        public Guid VideoId { get; set; }
        public Guid? PartFile { get; set; }=Guid.Empty;
        public DateTime ReleaseTime { get; set; }

        public PartStatuses PartStatus { get; set; } = PartStatuses.Public;

        public int SortIndex { get; set; }

        public VodVideoPartDto()
        {
        }
        public VodVideoPartDto(VodVideoPart vodVideoPart)
        {
            Id=vodVideoPart.Id;
            PartName=vodVideoPart.PartName;
            VideoId = vodVideoPart.Video.Id;
            PartFile = vodVideoPart.PartFile;
            ReleaseTime= vodVideoPart.ReleaseTime;
            PartStatus = vodVideoPart.PartStatus;
            SortIndex = vodVideoPart.SortIndex;
        }

    }
    public class AddVodVideoPartDtoValidator : AbstractValidator<VodVideoPartDto>
    {
        public AddVodVideoPartDtoValidator()
        {
            RuleFor(d => d.PartName).NotNull().NotEmpty().WithMessage("视频片段名称不能空");
            RuleFor(d => d.VideoId).NotNull().NotEmpty().WithMessage("视频Id不能空");

        }
    }
    public class UpdateVodVideoPartDtoValidator : AbstractValidator<VodVideoPartDto>
    {
        public UpdateVodVideoPartDtoValidator()
        {
            RuleFor(d => d.Id).NotNull().NotEmpty().Must(id => id != Guid.Empty).WithMessage("Id不能空");
            RuleFor(d => d.PartName).NotNull().NotEmpty().WithMessage("视频片段名称不能空");
            RuleFor(d => d.VideoId).NotNull().NotEmpty().WithMessage("视频Id不能空");

        }
    }
}
