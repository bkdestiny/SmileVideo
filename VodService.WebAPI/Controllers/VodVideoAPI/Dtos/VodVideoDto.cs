using FluentValidation;
using VodService.Domain.Entities;
using VodService.WebAPI.Controllers.VodVideoClassifyAPI.Dtos;

namespace VodService.WebAPI.Controllers.VodVideoAPI.Dtos
{
    public class VodVideoDto
    {
        public Guid Id { get; set; }
        public string VideoName { get; set; }

        public Guid? CoverFile { get; set; } = new Guid();

        public string? Performers { get; set; } = "";

        public string? Director { get; set; } = "";

        public string? Scriptwriter { get; set; } = "";

        public string? Description { get; set; } = "";

        public string? Profile { get; set; } = "";

        public DateTime CreateTime {  get; set; }

        public VideoStatuses VideoStatus { get; set; } = VideoStatuses.Private;

        public List<VodVideoClassifyDto> VideoClassifies {  get; set; }


        public VodVideoDto() { }

        public VodVideoDto(VodVideo vodVideo)
        {
            Id = vodVideo.Id;
            VideoName = vodVideo.VideoName;
            CoverFile = vodVideo.CoverFile;
            Performers = vodVideo.Performers;
            Director = vodVideo.Director;
            Scriptwriter = vodVideo.Scriptwriter;
            Description = vodVideo.Description;
            Profile = vodVideo.Profile;
            CreateTime = vodVideo.CreateTime;
            VideoStatus = vodVideo.VideoStatus;
            VideoClassifies = vodVideo.VideoClassifies.Select(e => new VodVideoClassifyDto(e)).ToList();
        }
    }


    public class VodVideoDtoValidator : AbstractValidator<VodVideoDto>
    {
        public VodVideoDtoValidator()
        {
            RuleFor(e => e.VideoName).NotNull().NotEmpty().WithMessage("视频名称不能空");
        }
    }
}
