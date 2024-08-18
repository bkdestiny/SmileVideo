using LiveService.Domain.Entites;
using Microsoft.AspNetCore.SignalR;

namespace LiveService.WebAPI.Controllers.LiveAPI.Dtos
{
    public class LiveRoomDto
    {
        public Guid UserId { get; set; }

        public string Title { get; set; } = "";

        public Guid? CoverFile { get; set; }

        public LiveRoomDto() { }

        public LiveRoomDto(LiveRoom liveRoom) 
        { 
            UserId = liveRoom.UserId;
            Title = liveRoom.Title;
            CoverFile = liveRoom.CoverFile;
        }
    }
}
