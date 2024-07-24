using FluentValidation;
using VodService.Domain.Entities;

namespace VodService.WebAPI.Controllers.VodVideoCommentAPI.Dtos
{
    public class VodVideoCommentDto
    {
        public Guid? Id { get; set; } = Guid.Empty;

        public Guid VideoId { get; set; }
        public Guid UserId { get; set; }=Guid.Empty;

        public Guid? RespondentUserId { get; set; } = Guid.Empty;

        public Guid RootVideoCommentId { get; set; } = Guid.Empty;

        public List<VodVideoCommentDto> SubVideoCommentList { get; set; }=new List<VodVideoCommentDto>();

        public string Content { get; set; }

        public DateTime CreateTime { get; private set; }

        public int LikesCount { get; private set; } = 0;


        public VodVideoCommentDto()
        {

        }

        public VodVideoCommentDto(VodVideoComment vodVideoComment)
        {
            this.Id= vodVideoComment.Id;
            this.VideoId = vodVideoComment.Video.Id;
            this.UserId= vodVideoComment.UserId;
            this.RespondentUserId= vodVideoComment.RespondentUserId;
            this.RootVideoCommentId = vodVideoComment.RootVideoComment!=null?vodVideoComment.RootVideoComment.Id:Guid.Empty;
            this.SubVideoCommentList = vodVideoComment.SubVideoComments.Select(c => new VodVideoCommentDto(c)).ToList();
            this.Content = vodVideoComment.Content;
            this.CreateTime= vodVideoComment.CreateTime;
            this.LikesCount = vodVideoComment.LikesCount;
        }
    }
    public class AddRootVodVideoCommentDtoValidator : AbstractValidator<VodVideoCommentDto>
    {
        public AddRootVodVideoCommentDtoValidator()
        {
            //RuleFor(e => e.UserId).NotNull().NotEmpty().WithMessage("用户Id不能为空");
            RuleFor(e => e.Content).NotNull().NotEmpty().Must(content=>content.Trim().Length!=0).WithMessage("评论内容不能为空").Must(content=>content.Length<=1000).WithMessage("评论内容过多");
            RuleFor(e => e.VideoId).NotNull().NotEmpty().WithMessage("视频Id不能为空");
        }
    }

    public class AddSubVodVideoCommentDtoValidator : AbstractValidator<VodVideoCommentDto>
    {
        public AddSubVodVideoCommentDtoValidator()
        {
            //RuleFor(e => e.UserId).NotNull().NotEmpty().WithMessage("用户Id不能为空");
            RuleFor(e => e.Content).NotNull().NotEmpty().Must(content => content.Trim().Length != 0).WithMessage("评论内容不能为空").Must(content => content.Length <= 1000).WithMessage("评论内容过多");
            RuleFor(e => e.VideoId).NotNull().NotEmpty().WithMessage("视频Id不能为空");
            RuleFor(e => e.RootVideoCommentId).NotNull().NotEmpty().WithMessage("根评论Id不能空");
        }
    }
    public class AddAnswerSubVodVideoCommentDtoValidator : AbstractValidator<VodVideoCommentDto>
    {
        public AddAnswerSubVodVideoCommentDtoValidator()
        {
            //RuleFor(e => e.UserId).NotNull().NotEmpty().WithMessage("用户Id不能为空");
            RuleFor(e => e.Content).NotNull().NotEmpty().Must(content => content.Trim().Length != 0).WithMessage("评论内容不能为空").Must(content => content.Length <= 1000).WithMessage("评论内容过多");
            RuleFor(e => e.VideoId).NotNull().NotEmpty().WithMessage("视频Id不能为空");
            RuleFor(e => e.RootVideoCommentId).NotNull().NotEmpty().WithMessage("根评论Id不能空");
            RuleFor(e=>e.RespondentUserId).NotNull().NotEmpty().WithMessage("回复用户Id不能为空");
        }
    }
}
