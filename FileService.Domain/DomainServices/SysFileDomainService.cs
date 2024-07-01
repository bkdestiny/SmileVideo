using Common.Utils;
using FileService.Domain.Entites;
using FileService.Domain.IRepositories;
using FileService.Domain.IStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FileService.Domain.DomainServices
{
    public class SysFileDomainService
    {
        private readonly string tempFileRootDir;

        private readonly IStorageService storageService;

        private readonly ILogger<SysFileDomainService> logger;

        private readonly ISysFileRepository sysFileRepository;


        public SysFileDomainService(IConfiguration configuration, IStorageService storageService, ILogger<SysFileDomainService> logger, ISysFileRepository sysFileRepository)
        {
            tempFileRootDir = configuration.GetValue<string>("TempFileRootDir")!;
            this.storageService = storageService;
            this.logger = logger;
            this.sysFileRepository = sysFileRepository;
        }
        /// <summary>
        /// 上传文件到云存储服务器并且保存文件到数据库表
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<Guid?> SaveFileAsync(IFormFile file)
        {
            try
            {
                Stream stream = file.OpenReadStream();
                using (stream)
                {
                    //查询SysFile表是否存在相同文件
                    string fileSHA256Hash = HashHelper.ComputeSha256Hash(stream);
                    long fileSize = stream.Length;
                    SysFile? sysFile = await sysFileRepository.FindSysFileByFileSHA56HashAndFileSizeAsync(fileSHA256Hash, fileSize);
                    if (sysFile != null)
                    {
                        //文件已存在,直接返回文件ID
                        return sysFile.Id;
                    }
                    //SysFile表不存在该文件,需要新增该文件
                    //1.先复制到本地文件  
                    DateTime now = DateTime.Now;
                    string localFolderPath = $"{tempFileRootDir}/temp/{now:yyyy-MM-dd}/{fileSHA256Hash}";
                    Stream? localStream = FileHelper.Create(localFolderPath, file.FileName);

                    if (localStream == null)
                    {
                        return null;
                    }
                    using (localStream)
                    {
                        //及时释放流对象,否则后面不能删除
                        await file.CopyToAsync(localStream);
                    }
                    string localFileFullPath = localFolderPath + "/" + file.FileName;

                    //2.上传到云存储服务器
                    string key = $"{now.Year}/{now.Month}/{now.Day}/{fileSHA256Hash}/{file.FileName}";
                    Uri remoteUrl = await storageService.SaveAsync(key, localFileFullPath);
                    if (remoteUrl == null)
                    {
                        return null;
                    }
                    //TODO 上传到备份存储服务器      
                    Uri backupUrl = remoteUrl;

                    //3.上传成功,删除本地刚创建的文件夹以及文件
                    FileHelper.DeleteFolder(localFolderPath);

                    //4.保存文件信息到数据库
                    sysFile = new SysFile(file.FileName, file.ContentType, file.Length, fileSHA256Hash, remoteUrl, backupUrl, new Guid());
                    await sysFileRepository.AddSysFileAsync(sysFile);
                    return sysFile.Id;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 根据Id获取文件的存储地址
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Uri?> GetSysFileUrl(Guid id)
        {
            SysFile? sysFile=await sysFileRepository.FindSysFileById(id);
            if (sysFile == null)
            {
                return null;
            }
            return sysFile.RemoteUrl;
        }
    }
}
