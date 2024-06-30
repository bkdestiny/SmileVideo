using FileService.Domain.Entites;
using FileService.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure.Repositories
{
    public class SysFileRepository : ISysFileRepository
    {
        private readonly SysFileDbContext sysFileDbContext;

        public SysFileRepository(SysFileDbContext sysFileDbContext)
        {
            this.sysFileDbContext = sysFileDbContext;
        }

        public async Task AddSysFileAsync(SysFile sysFile)
        {
            await sysFileDbContext.SysFiles.AddAsync(sysFile);
            await sysFileDbContext.SaveChangesAsync();
        }

        public async Task<SysFile?> FindSysFileByFileSHA56HashAndFileSizeAsync(string fileSHA256Hash, long fileSize)
        {
            return await sysFileDbContext.SysFiles.Where(f => f.FileSHA256Hash == fileSHA256Hash && f.FileSize == fileSize).FirstOrDefaultAsync<SysFile>();
        }
    }
}
