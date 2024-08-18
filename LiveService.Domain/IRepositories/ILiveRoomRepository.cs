using LiveService.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveService.Domain.IRepositories
{
    public interface ILiveRoomRepository
    {
        Task<IList<LiveRoom>> QueryLiveRoomAsync();

        Task AddLiveRoomAsync(LiveRoom room);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">直播间Id或者用户Id</param>
        /// <returns></returns>
        Task<LiveRoom?> GetLiveRoomByIdAsync(Guid id);

    }
}
