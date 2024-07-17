using Common.Models;
using Common.Utils;
using FileService.Domain.Entites;
using FileService.Domain.IRepositories;
using FileService.Domain.IStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Runtime.CompilerServices;

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
        public async Task<Guid> SaveFileAsync(IFormFile file)
        {
            string tempFolderPath = "";
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
                    tempFolderPath = $"{tempFileRootDir}/{now:yyyy-MM-dd}/{fileSHA256Hash}";
                    Stream? localStream = FileHelper.Create(tempFolderPath, file.FileName);

                    if (localStream == null)
                    {
                        return Guid.Empty;
                    }
                    using (localStream)
                    {
                        //及时释放流对象,否则后面不能删除
                        await file.CopyToAsync(localStream);
                    }
                    string tempFileFullPath = tempFolderPath + "/" + file.FileName;

                    //2.上传到云存储服务器
                    string key = $"{now.Year}/{now.Month}/{now.Day}/{fileSHA256Hash}/{file.FileName}";
                    Uri remoteUrl = await storageService.SaveAsync(key, tempFileFullPath);
                    if (remoteUrl == null)
                    {
                        return Guid.Empty;
                    }
                    //TODO 上传到备份存储服务器      
                    Uri backupUrl = remoteUrl;

                    //3.上传成功,删除本地刚创建的文件夹以及文件
                    

                    //4.保存文件信息到数据库
                    sysFile = new SysFile(file.FileName, file.ContentType, file.Length, fileSHA256Hash, remoteUrl, backupUrl, new Guid());
                    await sysFileRepository.AddSysFileAsync(sysFile);
                    return sysFile.Id;
                }
            }
            catch (Exception ex)
            {
                return Guid.Empty;
            }
            finally
            {
                //删除临时文件夹以及内容
                FileHelper.DeleteFolder(tempFolderPath);
            }
        }
        /// <summary>
        /// 转换文件格式为hls并上传
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<Guid> ConvertHlsAndSaveFile(IFormFile file)
        {
            string tempFolderPath = "";
            try
            {
                Stream stream = file.OpenReadStream();
                using (stream)
                {
                    logger.LogInformation(file.FileName+" 视频转换为Hls并上传开始");
                    string fileSHA256Hash = HashHelper.ComputeSha256Hash(stream);
                    long fileSize = stream.Length;
                    SysFile? sysFile = await sysFileRepository.FindSysFileByFileSHA56HashAndFileSizeAsync(fileSHA256Hash, fileSize);
                    if (sysFile != null)
                    {
                        //文件已存在,直接返回文件ID
                        logger.LogInformation(file.FileName + " 视频已存在 返回文件ID:"+sysFile.Id);
                        return sysFile.Id;
                    }
                    //1.先复制到本地临时文件夹  
                    DateTime now = DateTime.Now;
                    tempFolderPath = $"{tempFileRootDir}/{now:yyyy-MM-dd}/{Guid.NewGuid()}";
                    Stream? tempStream = FileHelper.Create(tempFolderPath, file.FileName);
                    if (tempStream == null)
                    {
                        return Guid.Empty;
                    }
                    logger.LogInformation(file.FileName + " 复制到本地临时文件夹:"+tempFolderPath);
                    using (tempStream)
                    {
                        //及时释放流对象,否则后面不能删除
                        await file.CopyToAsync(tempStream);
                    }
                    string tempFileFullPath = tempFolderPath + "/" + file.FileName;//mp4输入视频文件
                    string hlsFolderPath = tempFolderPath + "/hls";//hls输出文件夹
                    //转换为hls
                    logger.LogInformation(file.FileName + " 开始转换为Hls到临时文件夹:" + hlsFolderPath);
                    string? tempM3u8Path=FfmpegHelper.ConvertMp4ToHls(tempFileFullPath, hlsFolderPath);
                    if (string.IsNullOrEmpty(tempFileFullPath))
                    {
                        return Guid.Empty;
                    }
                    logger.LogInformation(file.FileName + ",转换为Hls完成");
                    string prefixKey = $"{now.Year}/{now.Month}/{now.Day}/{fileSHA256Hash}";
                    //3.上传Hls文件夹到云服务器
                    logger.LogInformation(file.FileName + " 开始上传到云服务器");
                    Uri remoteUrl= await storageService.SaveHlsAsync(prefixKey, hlsFolderPath);
                    if (remoteUrl == null)
                    {
                        return Guid.Empty;
                    }
                    logger.LogInformation(file.FileName + " 上传到云服务器完成");
                    Uri backupUrl = remoteUrl;

                    //4.保存文件信息到数据库
                    logger.LogInformation(file.FileName + " 开始保存到数据库");
                    sysFile = new SysFile(file.FileName, file.ContentType, file.Length, fileSHA256Hash, remoteUrl, backupUrl, new Guid(),false);
                    await sysFileRepository.AddSysFileAsync(sysFile);
                    logger.LogInformation(file.FileName + " 保存到数据库完成");
                    return sysFile.Id;
                }  
            }
            catch (Exception ex) {
                logger.LogError("转换视频文件为Hls失败,错误信息:"+ex.Message);
                return Guid.Empty;
            }
            finally
            {
                //删除临时文件夹以及内容
                try
                {
                    FileHelper.DeleteFolder(tempFolderPath);
                }catch(Exception e)
                {
                    Console.WriteLine("删除文件夹失败," + e.Message);
                }
                
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
        /// <summary>
        /// 查询系统文件
        /// </summary>
        /// <param name="fileIds"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SysFile>> QuerySysFileAsync(Guid[] fileIds)
        {
            return await sysFileRepository.QuerySysFileAsync(fileIds);
        }
    }
}
