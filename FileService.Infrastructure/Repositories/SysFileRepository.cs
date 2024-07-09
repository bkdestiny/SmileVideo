using FileService.Domain.Entites;
using FileService.Domain.IRepositories;
using MediatR.NotificationPublishers;
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
        }

        public async Task<SysFile?> FindSysFileByFileSHA56HashAndFileSizeAsync(string fileSHA256Hash, long fileSize)
        {
            return await sysFileDbContext.SysFiles.Where(f => f.FileSHA256Hash == fileSHA256Hash && f.FileSize == fileSize).FirstOrDefaultAsync<SysFile>();
        }

        public async Task<SysFile?> FindSysFileById(Guid id)
        {
            return await sysFileDbContext.SysFiles.Where(f=>f.Id == id).FirstOrDefaultAsync<SysFile>();
        }

        public async Task<IEnumerable<SysFile>> QuerySysFileAsync(Guid[] ids)
        {
            return await sysFileDbContext.SysFiles.Where(f=>ids.Contains(f.Id)).ToListAsync();
        }
    }
}
