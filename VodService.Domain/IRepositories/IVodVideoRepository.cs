using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VodService.Domain.Entities;

namespace VodService.Domain.IRepositories
{
    public interface IVodVideoRepository
    {
        Task<List<VodVideo>> FindVodVideoByVideoNameAsync(string videoName);
        Task AddVodVideoAsync(VodVideo vodVideo);

        void UpdateVodVideo(VodVideo vodVideo);

        IQueryable<VodVideo> VodVideoQueryable();

        Task<VodVideo?> FindVodVideoById(Guid id);
    }
}
