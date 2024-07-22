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
    public class VodVideoCommentRepository : IVodVideoCommentRepository
    {
        private readonly VodDbContext vodDbContext;

        public VodVideoCommentRepository(VodDbContext vodDbContext)
        {
            this.vodDbContext = vodDbContext;
        }

        public async Task AddVodVideoCommentAsync(VodVideoComment vodVideoComment)
        {

            await vodDbContext.VodVideoComments.AddAsync(vodVideoComment);
        }

        public async Task<VodVideoComment?> FindVodVideoCommentByIdAsync(Guid id)
        {
            return await vodDbContext.VodVideoComments.SingleOrDefaultAsync(e=>e.Id == id);
        }
    }
}
