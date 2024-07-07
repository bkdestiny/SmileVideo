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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="classifyId">分类Id</param>
        /// <returns></returns>
        Task<IEnumerable<VodVideo>> QueryVodVideoAsync(Guid? classifyId);

        Task<VodVideo?> FindVodVideoById(Guid id);

        Task BindVodVideoClassify(Guid videoId,Guid classifyIds);

        Task ClearBindVodVideoClassify(Guid id);

        Task<List<VodVideo>?> Test(Guid id);
    }
}
