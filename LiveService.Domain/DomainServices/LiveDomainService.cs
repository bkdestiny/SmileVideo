using LiveService.Domain.Entites;
using LiveService.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveService.Domain.DomainServices
{
    public class LiveDomainService
    {
        private readonly ILiveRoomRepository liveRepository;

        public LiveDomainService(ILiveRoomRepository liveRepository)
        {
            this.liveRepository = liveRepository;
        }
        /// <summary>
        /// 创建直播间
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<(bool,string)> CreateLiveRoomAsync(Guid userId)
        {
            LiveRoom? liveRoom=await liveRepository.GetLiveRoomByIdAsync(userId);
            if (liveRoom != null)
            {
                return (false, "该用户已创建直播间");
            }
            liveRoom= new LiveRoom(userId);
            await liveRepository.AddLiveRoomAsync(liveRoom);  
            return (true, "");
        }
    }
}
