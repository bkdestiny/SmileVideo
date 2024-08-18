using Common;
using Common.Attributes;
using Common.Models;
using Exceptionless.Models.Data;
using LiveService.Domain.DomainServices;
using LiveService.Domain.Entites;
using LiveService.Domain.IRepositories;
using LiveService.Infrastructure;
using LiveService.Infrastructure.Repositories;
using LiveService.WebAPI.Controllers.LiveAPI.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LiveService.WebAPI.Controllers.LiveAPI
{
    [Route("LiveRoom")]
    [ApiController]
    [UnitOfWork([typeof(LiveDbContext)])]
    [Authorize]
    public class LiveAPIController:ControllerBase
    {
        private readonly LiveDomainService liveDomainService;

        private readonly ILiveRoomRepository liveRoomRepository;

        public LiveAPIController(LiveDomainService liveDomainService, ILiveRoomRepository liveRoomRepository)
        {
            this.liveDomainService = liveDomainService;
            this.liveRoomRepository = liveRoomRepository;
        }
        /// <summary>
        /// 角色Streamer创建直播间
        /// </summary>
        /// <returns></returns>
        [HttpPost("CreateLiveRoom")]
        public async Task<ActionResult<Result>> CreateLiveRoom()
        {
            if (!UserContext.UserInfo!.HasRole("Streamer"))
            {
                return Result.Error("未获取直播权限");
            }
            Guid userId = UserContext.UserInfo!.Id;
            (bool success,string message)=await liveDomainService.CreateLiveRoomAsync(userId);
            if (!success)
            {
                return Result.Error(message);
            }
            return Result.Ok();
        }
        /// <summary>
        /// 获取直播间信息
        /// </summary>
        /// <param name="Id">用户Id或直播间Id</param>
        /// <returns></returns> 
        [HttpGet("GetLiveRoom")]
        public async Task<ActionResult<Result>> GetLiveRoom([FromQuery]Guid Id)
        {
            LiveRoom? liveRoom=await liveRoomRepository.GetLiveRoomByIdAsync(Id);
            if(liveRoom == null)
            {
                return Result.Error("不存在该直播间");
            }
            LiveRoomDto dto=new LiveRoomDto(liveRoom);
            return Result.Ok(dto);
        }
    }
}
