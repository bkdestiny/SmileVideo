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
    public class VodVideoRepository : IVodVideoRepository
    {
        private readonly VodDbContext vodDbContext;

        public VodVideoRepository(VodDbContext vodDbContext)
        {
            this.vodDbContext = vodDbContext;
        }

        

        public async Task AddVodVideoAsync(VodVideo vodVideo)
        {
            await vodDbContext.VodVideos.AddAsync(vodVideo);
        }

        public async Task BindVodVideoClassify(Guid videoId,Guid classifyId)
        {
            VodVideo? vodVideo=await vodDbContext.VodVideos.Include(v=>v.VideoClassifies).FirstOrDefaultAsync(v=>v.Id==videoId);
            VodVideoClassify? vodVideoClassify = await vodDbContext.VodVideoClassifies.FirstOrDefaultAsync(c => c.Id == classifyId);
            if (vodVideo != null && vodVideoClassify != null) {
                vodVideo.VideoClassifies.Add(vodVideoClassify);
            }
            
        }

        public async Task ClearBindVodVideoClassify(Guid id)
        {
            VodVideo video=await vodDbContext.VodVideos.Include(v=>v.VideoClassifies).SingleAsync(v => v.Id == id);
            video.VideoClassifies.Clear();
        }

        public async Task<VodVideo?> FindVodVideoByIdAsync(Guid id)
        {
            return await vodDbContext.VodVideos.Where(v => v.Id == id && !v.IsDeleted).Include(v=>v.VideoClassifies).FirstOrDefaultAsync();
        }

        public async Task<List<VodVideo>> FindVodVideoByVideoNameAsync(string videoName)
        {
            return await vodDbContext.VodVideos.Where(e => e.VideoName.Equals(videoName) && e.IsDeleted == false).ToListAsync();
        }

        public void UpdateVodVideo(VodVideo vodVideo)
        {
            vodDbContext.Update(vodVideo);
        }

        public async Task<IEnumerable<VodVideo>> QueryVodVideoAsync(List<Guid> classifyIds,VideoStatuses? videoStatus, string? searchText)
        {
            return await vodDbContext.VodVideos
                .Where(e =>classifyIds .Count>0 ? classifyIds.All(id=>e.VideoClassifies.Any(c=>c.Id==id)): true && videoStatus!=null?e.VideoStatus==videoStatus:true)
                .Where(e=>!string.IsNullOrEmpty(searchText)?e.VideoName.Contains(searchText)||e.Profile.Contains(searchText)||e.Description.Contains(searchText):true)
                .Include(e => e.VideoClassifies).ToListAsync();
        }
        public async Task<List<VodVideo>?> Test(Guid id)
        {
            return await vodDbContext.VodVideos.Where(e => e.VideoClassifies.Any(c => c.Id == id)).Include(e => e.VideoClassifies).ToListAsync();
        }

        public async Task DeleteVodVideoByIdAsync(Guid id)
        {
            VodVideo? video = await vodDbContext.VodVideos.SingleAsync(v => v.Id == id&&!v.IsDeleted);
            if (video != null)
            {
                video.SoftDelete();
            }
            
        }
    }
}
