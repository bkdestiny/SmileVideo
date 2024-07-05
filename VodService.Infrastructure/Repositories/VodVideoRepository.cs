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

        public async Task<VodVideo?> FindVodVideoById(Guid id)
        {
            return await vodDbContext.VodVideos.Where(v => v.Id == id && !v.IsDeleted).SingleOrDefaultAsync();
        }

        public async Task<List<VodVideo>> FindVodVideoByVideoNameAsync(string videoName)
        {
            return await vodDbContext.VodVideos.Where(e => e.VideoName.Equals(videoName) && e.IsDeleted == false).ToListAsync();
        }

        public void UpdateVodVideo(VodVideo vodVideo)
        {
            vodDbContext.Update(vodVideo);
        }

        public IQueryable<VodVideo> VodVideoQueryable()
        {
            return vodDbContext.VodVideos;
        }
    }
}
