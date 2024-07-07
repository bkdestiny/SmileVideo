using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VodService.Domain.Entities;

namespace VodService.Domain.IRepositories
{
    public interface IVodVideoClassifyRepository
    {
        IQueryable<VodVideoClassify> VodVideoClassifyfQueryable();

        Task AddVodVideoClassifyAsync(VodVideoClassify vodVideoClassify);

        void UpdateVodVideoClassify(VodVideoClassify vodVideoClassify);

        Task<VodVideoClassify?> FindVodVideoClassifyByIdAsync(Guid id);

        Task<List<VodVideoClassify>> FindVodVideoClassifyByClassifyNameAsync(string name);

        /// <summary>
        /// 分类类型
        /// </summary>
        /// <param name="classifyType"></param>
        /// <returns></returns>
        Task<IEnumerable<VodVideoClassify>> QueryVodVideoClassifyAsync(ClassifyTypes? classifyType);
    }
}
