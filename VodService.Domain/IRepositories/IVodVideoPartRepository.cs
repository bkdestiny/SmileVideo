using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VodService.Domain.Entities;

namespace VodService.Domain.IRepositories
{
    public interface IVodVideoPartRepository
    {
        Task AddVodVideoPartAsync(VodVideoPart vodVideoPart);

        void UpdateVodVideoPart(VodVideoPart vodVideoPart);

        Task<VodVideoPart?> GetVodVideoPartByIdAsync(Guid id);

        Task<IEnumerable<VodVideoPart>> QueryVodVideoPartAsync(Guid videoId,string? searchText);

        Task BindVodVideoAsync(Guid videoId,Guid partId);

        Task RemoveVodVideoPartByIdAsync(Guid id);
    }
}
