using COSXML;
using COSXML.Auth;
using COSXML.Transfer;
using FileService.Domain.IStorage;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure.Storage
{
    public class TencentYunCosService : IStorageService
    {
        private readonly CosXmlConfig config;

        private readonly TencentYunCosOptions options;

        private readonly QCloudCredentialProvider cosCredentialProvider;

        private readonly CosXml cosXml;

        public TencentYunCosService(TencentYunCosOptions options)
        {
            this.options = options;
            config = new CosXmlConfig.Builder()
            .IsHttps(true)  //设置默认 HTTPS 请求
            .SetRegion(options.region)  //设置一个默认的存储桶地域
            .SetDebugLog(true)  //显示日志
            .Build();  //创建 CosXmlConfig 对象
            //secretId 用户的 SecretId，建议使用子账号密钥，授权遵循最小权限指引，降低使用风险。
            //secretKey 用户的 SecretKey，建议使用子账号密钥，授权遵循最小权限指引，降低使用风险
            //durationSecond 每次请求签名有效时长，单位为秒
            this.cosCredentialProvider = new DefaultQCloudCredentialProvider(options.secretId, options.secretKey, 600);
            ///使用 CosXmlConfig 与 QCloudCredentialProvider 初始化 CosXmlServer 服务类。服务类建议在程序中作为单例使用
            cosXml = new CosXmlServer(config, cosCredentialProvider);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">对象在存储桶中的位置标识符，即称对象键</param>
        /// <param name="srcPath">本地文件绝对路径</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Uri> SaveAsync(string key, string srcPath)
        {
            // 初始化 TransferConfig
            TransferConfig transferConfig = new TransferConfig();
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            String bucket = options.buckets.Default; //存储桶，格式：BucketName-APPID
            // 上传对象
            COSXMLUploadTask uploadTask = new COSXMLUploadTask(bucket, key);
            uploadTask.SetSrcPath(srcPath);
            uploadTask.progressCallback = delegate (long completed, long total)
            {
                Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
            };
            try
            {
                COSXML.Transfer.COSXMLUploadTask.UploadTaskResult result = await transferManager.UploadAsync(uploadTask);
                //生成文件Url
                string url = cosXml.GetObjectUrl(bucket, key);
                return new Uri(url);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<Uri> SaveHlsAsync(string prefixKey, string srcPath)
        {
            if (!Directory.Exists(srcPath))
            {
                throw new Exception(srcPath + "不存在或者不是一个文件夹");
            }
            IEnumerable<string> files = Directory.GetFiles(srcPath);
            string? m3u8FilePath = files.Where(f => f.IndexOf(".m3u8") != -1).FirstOrDefault();
            List<string> tsFilePathList = files.Where(f => f.IndexOf(".ts") != -1).ToList();
            if (string.IsNullOrEmpty(m3u8FilePath) || tsFilePathList.Count == 0)
            {
                throw new Exception(srcPath + "不存在.m3u8文件或.ts文件");
            }
            // 初始化 TransferConfig
            TransferConfig transferConfig = new TransferConfig();
            // 初始化 TransferManager
            TransferManager transferManager = new TransferManager(cosXml, transferConfig);
            String bucket = options.buckets.Default; //存储桶，格式：BucketName-APPID
            // 上传对象
            try
            {
                //1.上传M3u8
                string m3u8FileName = new FileInfo(m3u8FilePath).Name;
                string m3u8FileKey = prefixKey + "/" + m3u8FileName;
                COSXMLUploadTask uploadM3u8Task = new COSXMLUploadTask(bucket, m3u8FileKey);
                uploadM3u8Task.SetSrcPath(m3u8FilePath);
                COSXML.Transfer.COSXMLUploadTask.UploadTaskResult result = await transferManager.UploadAsync(uploadM3u8Task);
                uploadM3u8Task.progressCallback = delegate (long completed, long total)
                {
                    Console.WriteLine(String.Format(m3u8FileName+":上传进度 = {0:##.##}%", completed * 100.0 / total));
                };
                if (!result.IsSuccessful())
                {
                    throw new Exception(".m3u8文件上传Cos失败,"+result.httpMessage);
                }
                Console.WriteLine(String.Format(m3u8FileName + ":上传完成"));
                //生成文件Url
                string m3u8Url = cosXml.GetObjectUrl(bucket, m3u8FileKey);

                int tsCount = tsFilePathList.Count;
                int tsIndex = 0;
                //2.批量上传ts
                foreach (string tsFilePath in tsFilePathList)
                {
                    tsIndex++;
                    string tsFileName = new FileInfo(tsFilePath).Name;
                    string tsFileKey = prefixKey + "/" + tsFileName;
                    COSXMLUploadTask uploadTsTask = new COSXMLUploadTask(bucket, tsFileKey);
                    uploadTsTask.SetSrcPath(tsFilePath);
                    COSXML.Transfer.COSXMLUploadTask.UploadTaskResult uploadTsResult = await transferManager.UploadAsync(uploadTsTask);
                    uploadTsTask.progressCallback = delegate (long completed, long total)
                    {
                        Console.WriteLine(String.Format(m3u8FileName + ":上传进度 = {0:##.##}%,批量上传进度="+tsIndex+"/"+tsCount+"", completed * 100.0 / total));
                    };
                    if (!uploadTsResult.IsSuccessful())
                    {
                        throw new Exception(".ts文件上传Cos失败,"+uploadTsResult.httpMessage);
                    }
                    Console.WriteLine(String.Format(tsFileName + ":上传完成"));
                }
                return new Uri(m3u8Url);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
