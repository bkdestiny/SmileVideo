using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VodService.Domain.Entities;
using VodService.Domain.IRepositories;

namespace VodService.Infrastructure.Repositories
{
    public class VodVideoPartRepository : IVodVideoPartRepository
    {
        private readonly VodDbContext vodDbContext;

        public VodVideoPartRepository(VodDbContext vodDbContext)
        {
            this.vodDbContext = vodDbContext;
        }

        public async Task AddVodVideoPartAsync(VodVideoPart vodVideoPart)
        {
            await vodDbContext.VodVideoParts.AddAsync(vodVideoPart);
        }

        public async Task BindVodVideoAsync(Guid videoId, Guid partId)
        {
            VodVideo? vodVideo=await vodDbContext.VodVideos.FirstOrDefaultAsync(v => v.Id == videoId);
            VodVideoPart? vodVideoPart=await vodDbContext.VodVideoParts.Include(p=>p.Video).FirstOrDefaultAsync(p => p.Id == partId);
            if (vodVideo!=null&&vodVideoPart!=null)
            {
                vodVideoPart.Video = vodVideo;
            }
        }

        public async Task<VodVideoPart?> GetVodVideoPartByIdAsync(Guid id)
        {
            return await vodDbContext.VodVideoParts.Include(p=>p.Video).Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<VodVideoPart>> QueryVodVideoPartAsync(Guid videoId, string? searchText)
        {
            return await vodDbContext.VodVideoParts.Where(p => p.Video.Id == videoId).Where(e=>!string.IsNullOrEmpty(searchText)?e.PartName.Contains(searchText):true).Include(p=>p.Video).AsNoTracking().ToListAsync() ;
        }

        public async Task RemoveVodVideoPartByIdAsync(Guid id)
        {
            VodVideoPart? videoPart = await vodDbContext.VodVideoParts.SingleOrDefaultAsync(e => e.Id == id);
            if (videoPart != null)
            {
                vodDbContext.VodVideoParts.Remove(videoPart);
            }
        }

        public void UpdateVodVideoPart(VodVideoPart vodVideoPart)
        {
            vodDbContext.Update(vodVideoPart);
        }
    }
}
