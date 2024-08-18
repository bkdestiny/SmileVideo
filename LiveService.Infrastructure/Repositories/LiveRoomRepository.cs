using LiveService.Domain.Entites;
using LiveService.Domain.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveService.Infrastructure.Repositories
{
    public class LiveRoomRepository : ILiveRoomRepository
    {
        private readonly LiveDbContext dbContext;

        public LiveRoomRepository(LiveDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddLiveRoomAsync(LiveRoom room)
        {
            await dbContext.LiveRooms.AddAsync(room);
        }

        public async Task<LiveRoom?> GetLiveRoomByIdAsync(Guid id)
        {
            return await dbContext.LiveRooms.SingleOrDefaultAsync(e=>e.UserId == id||e.Id==id);
        }

        public async Task<IList<LiveRoom>> QueryLiveRoomAsync()
        {
            return await dbContext.LiveRooms.AsNoTracking().ToListAsync();
        }
    }
}
