using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VodService.Domain.Entities;

namespace VodService.Domain.IRepositories
{
    public interface IVodVideoCommentRepository
    {
        Task AddVodVideoCommentAsync(VodVideoComment vodVideoComment);

        Task<VodVideoComment?> FindVodVideoCommentByIdAsync(Guid id); 
        
    }
}
