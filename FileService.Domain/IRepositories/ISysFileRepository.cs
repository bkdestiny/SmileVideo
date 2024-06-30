using FileService.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain.IRepositories
{
    public interface ISysFileRepository
    {
        /// <summary>
        /// 根据FileSHA256Hash和文件大小查找目标文件
        /// </summary>
        /// <param name="fileSHA256Hash"></param>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        Task<SysFile?> FindSysFileByFileSHA56HashAndFileSizeAsync(string fileSHA256Hash, long fileSize);

        /// <summary>
        /// 新增SysFile
        /// </summary>
        /// <param name="sysFile"></param>
        /// <returns></returns>
        Task AddSysFileAsync(SysFile sysFile);
    }
}
