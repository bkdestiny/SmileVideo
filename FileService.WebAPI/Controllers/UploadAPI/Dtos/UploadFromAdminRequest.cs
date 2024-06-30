using FluentValidation;

namespace FileService.WebAPI.Controllers.UploadAPI.Dtos
{
    public class UploadFromAdminRequest
    {
        //不要声明为Action的参数，否则不会正常工作
        public IFormFile File { get; set; }
    }

    public class UploadFromAdminRequestValidaror : AbstractValidator<UploadFromAdminRequest>
    {
        public UploadFromAdminRequestValidaror()
        {
            RuleFor(e => e.File).NotNull().Must(f=>f.Length>0).WithMessage("不能上传空文件");
        }
    }
}
