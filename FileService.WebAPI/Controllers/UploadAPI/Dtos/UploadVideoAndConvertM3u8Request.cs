using FluentValidation;

namespace FileService.WebAPI.Controllers.UploadAPI.Dtos
{
    public class UploadVideoAndConvertM3u8Request
    {
        public IFormFile File { get; set; }
    }
    public class UploadVideoAndConvertM3u8RequestValidator : AbstractValidator<UploadVideoAndConvertM3u8Request>
    {
        public UploadVideoAndConvertM3u8RequestValidator()
        {
            RuleFor(e => e.File).NotNull().Must(f => f.Length > 0).WithMessage("不能上传空文件").Must(f => f.FileName.LastIndexOf(".") != -1).Must(f => f.FileName.Substring(f.FileName.LastIndexOf(".") + 1) == "mp4").WithMessage("要求文件扩展名为mp4");
        }
    }
}
