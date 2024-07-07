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
    public class VodVideoClassifyRepository : IVodVideoClassifyRepository
    {
        private readonly VodDbContext vodDbContext;

        public VodVideoClassifyRepository(VodDbContext vodDbContext)
        {
            this.vodDbContext = vodDbContext;
        }

        public async Task AddVodVideoClassifyAsync(VodVideoClassify vodVideoClassify)
        {
            await vodDbContext.VodVideoClassifies.AddAsync(vodVideoClassify);
        }

        public async Task<List<VodVideoClassify>> FindVodVideoClassifyByClassifyNameAsync(string name)
        {
            return await vodDbContext.VodVideoClassifies.Where(e=>e.ClassifyName== name).ToListAsync();
        }

        public async Task<VodVideoClassify?> FindVodVideoClassifyByIdAsync(Guid id)
        {
            return await vodDbContext.VodVideoClassifies.Where(e => e.Id == id).SingleOrDefaultAsync();
        }

        public void UpdateVodVideoClassify(VodVideoClassify vodVideoClassify)
        {
            vodDbContext.Update(vodVideoClassify);
        }

        public IQueryable<VodVideoClassify> VodVideoClassifyfQueryable()
        {
            return vodDbContext.VodVideoClassifies;
        }

        public async Task<IEnumerable<VodVideoClassify>> QueryVodVideoClassifyAsync(ClassifyTypes? classifyType)
        {
            return await vodDbContext.VodVideoClassifies.Where(e => classifyType!=null?e.ClassifyType == classifyType:1==1).ToListAsync();
        }
    }
}
