using FluentValidation;

namespace FileService.WebAPI.Controllers.UploadAPI.Dtos
{
    public class UploadVideoAndConvertHlsRequest
    {
        public IFormFile File { get; set; }
    }
    public class UploadVideoAndConvertHlsRequestValidator : AbstractValidator<UploadVideoAndConvertHlsRequest>
    {
        public UploadVideoAndConvertHlsRequestValidator()
        {
            RuleFor(e => e.File).NotNull().Must(f => f.Length > 0).WithMessage("不能上传空文件").Must(f => f.FileName.LastIndexOf(".") != -1).Must(f => f.FileName.Substring(f.FileName.LastIndexOf(".") + 1) == "mp4").WithMessage("要求文件扩展名为mp4");
        }
    }
}
