using FileService.Domain.Entites;

namespace FileService.WebAPI.Controllers.SysFileAPI.Dtos
{
    public class SysFileDto
    {
        public Guid Id { get; set; }

        public string FileName { get; set; }

        public Uri Url { get; set; }

        private SysFileDto() { }

        public SysFileDto(SysFile sysFile)
        {
            Id=sysFile.Id;
            FileName=sysFile.FileName;
            Url = sysFile.RemoteUrl;
        }
    }
}
