using Common.EFcore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain.Entites
{
    public class SysFile : BaseEntity, IHasCreateTime
    {

        /// <summary>
        /// 文件名称
        /// </summary>
        public string FileName {  get; private set; }

        /// <summary>
        /// 文件内容类型
        /// </summary>
        public string FileContentType { get; private set; }
        /// <summary>
        /// 文件大小 单位：byte
        /// </summary>
        public long FileSize { get;private set; }
        /// <summary>
        /// 两个文件的大小和散列值（SHA256）都相同的概率非常小。因此只要大小和SHA256相同，就认为是相同的文件。
        /// SHA256的碰撞的概率比MD5低很多。
        /// </summary>
        public string FileSHA256Hash { get; private set; }

        /// <summary>
        /// 上传的文件的供外部访问者访问的路径
        /// </summary>
        public Uri? RemoteUrl { get; private set; }

        /// <summary>
        /// 备份文件路径，因为可能会更换文件存储系统或者云存储供应商，因此系统会保存一份自有的路径。
        /// 备份文件一般放到内网的高速、稳定设备上，比如NAS等。
        /// </summary>
        public Uri? BackupUrl { get; private set; }
        /// <summary>
        /// 创建人Id
        /// </summary>
        public Guid CreatorId { get; private set; }

        public DateTime CreateTime { private set; get; }
        /// <summary>
        /// true 单个文件 false 文件夹
        /// </summary>
        public bool IsSingle {  get; private set; }


        public SysFile(string fileName, string fileContentType, long fileSize, string fileSHA256Hash, Uri remoteUrl,Uri backupUrl,Guid creatorId, bool isSingle=true)
        {
            FileName = fileName;
            FileContentType = fileContentType;
            FileSize = fileSize;
            FileSHA256Hash = fileSHA256Hash;
            RemoteUrl = remoteUrl;
            BackupUrl = backupUrl;
            CreatorId = creatorId;
            CreateTime = DateTime.Now;
            IsSingle = isSingle;
        }
    }
}
