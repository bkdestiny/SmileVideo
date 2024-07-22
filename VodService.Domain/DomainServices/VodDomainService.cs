using AsmResolver.PE.DotNet.ReadyToRun;
using Common.EFcore.Models;
using Common.Models;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VodService.Domain.Entities;
using VodService.Domain.IRepositories;

namespace VodService.Domain.DomainServices
{
    public class VodDomainService
    {
        private readonly IVodVideoRepository vodVideoRepository;

        private readonly IVodVideoClassifyRepository vodVideoClassifyRepository;

        private readonly IVodVideoPartRepository vodVideoPartRepository;

        private readonly IVodVideoCommentRepository vodVideoCommentRepository;

        public VodDomainService(IVodVideoRepository vodVideoRepository, IVodVideoClassifyRepository vodVideoClassifyRepository, IVodVideoPartRepository vodVideoPartRepository, IVodVideoCommentRepository vodVideoCommentRepository)
        {
            this.vodVideoRepository = vodVideoRepository;
            this.vodVideoClassifyRepository = vodVideoClassifyRepository;
            this.vodVideoPartRepository = vodVideoPartRepository;
            this.vodVideoCommentRepository = vodVideoCommentRepository;
        }
        /// <summary>
        /// 新增视频
        /// </summary>
        /// <param name="vodVideo"></param>
        /// <returns></returns>
        public async Task<(bool,string)> AddVodVideoAsync(VodVideo vodVideo,Guid[] videoClassifyIds)
        {
            List<VodVideo> videoList=await vodVideoRepository.FindVodVideoByVideoNameAsync(vodVideo.VideoName);
            if (videoList.Count>0)
            {
                //该视频名称已被使用
                return (false,"该视频名称已被使用"); 
            }
            //新增视频信息
            await vodVideoRepository.AddVodVideoAsync(vodVideo);
            //绑定视频分类信息
            foreach (Guid videoClassifyId in videoClassifyIds) {
                await vodVideoRepository.BindVodVideoClassify(vodVideo.Id, videoClassifyId);
            }
            return (true, "新增成功");
        }
        /// <summary>
        /// 更新视频
        /// </summary>
        /// <param name="vodVideo"></param>
        /// <returns></returns>
        public async Task<(bool, string)> UpdateVodVideoAsync(VodVideo vodVideo, Guid[] videoClassifyIds) {
/*            List<VodVideo> videoList = await vodVideoRepository.FindVodVideoByVideoNameAsync(vodVideo.VideoName);
            if (videoList.Count > 0 && videoList[0].Id!=vodVideo.Id)
            {
                //该视频名称已被使用
                return (false, "该视频名称已被使用");
            }*/
            //更新视频信息
            vodVideoRepository.UpdateVodVideo(vodVideo);
            //清除原本绑定的视频分类
            await vodVideoRepository.ClearBindVodVideoClassify(vodVideo.Id);
            //更新视频分类
            foreach (Guid videoClassifyId in videoClassifyIds) { 
                await vodVideoRepository.BindVodVideoClassify(vodVideo.Id,videoClassifyId);
            }
            return (true, "更新成功");
        }
        /// <summary>
        /// 查询视频
        /// </summary>
        /// <param name="classifyId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<VodVideo>> QueryVodVideoAsync(List<Guid> classifyIds,VideoStatuses? videoStatus,string? searchText)
        {
            IEnumerable<VodVideo> enumerable = await vodVideoRepository.QueryVodVideoAsync(classifyIds,videoStatus,searchText);
            return enumerable;
        }
        /// <summary>
        /// 根据Id获取视频
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<VodVideo?> GetVodVideoByIdAsync(Guid id)
        {
           return await vodVideoRepository.FindVodVideoByIdAsync(id);
        }
        /// <summary>
        /// 删除视频
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task RemoveVodVideoAsync(params Guid[] ids)
        {
            foreach (Guid id in ids)
            {
                await vodVideoRepository.DeleteVodVideoByIdAsync(id);
            }
        }

        /// <summary>
        /// 根据Id获取视频分类
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<VodVideoClassify?> GetVodVideoClassifyByIdAsync(Guid id) {
            return await vodVideoClassifyRepository.FindVodVideoClassifyByIdAsync(id);
        }
        /// <summary>
        /// 新增视频分类
        /// </summary>
        /// <param name="vodVideoClassify"></param>
        /// <returns></returns>
        public async Task<(bool,string)> AddVodVideoClassifyAsync(VodVideoClassify vodVideoClassify)
        {
/*            List<VodVideoClassify> classifyList=await vodVideoClassifyRepository.FindVodVideoClassifyByClassifyNameAsync(vodVideoClassify.ClassifyName);
            if (classifyList.Count > 0) {
                return (false, "该视频分类名称已被使用");
            }*/
            await vodVideoClassifyRepository.AddVodVideoClassifyAsync(vodVideoClassify);
            return (true, "");
        }
        /// <summary>
        /// 更新视频分类
        /// </summary>
        /// <param name="vodVideoClassify"></param>
        /// <returns></returns>
        public async Task<(bool,string)> UpdateVodVideoClassifyAsync(VodVideoClassify vodVideoClassify) {
/*            List<VodVideoClassify> classifyList = await vodVideoClassifyRepository.FindVodVideoClassifyByClassifyNameAsync(vodVideoClassify.ClassifyName);
            if (classifyList.Count > 0 && classifyList[0].Id!=vodVideoClassify.Id)
            {
                return (false, "该视频分类名称已被使用");
            }*/
            vodVideoClassifyRepository.UpdateVodVideoClassify(vodVideoClassify);
            return (true, "更新成功");
        }
        /// <summary>
        /// 查询视频分类
        /// </summary>
        /// <param name="classifyType">分类类型</param>
        /// <returns></returns>
        public async Task<IEnumerable<VodVideoClassify>> QueryVodVideoClassifyAsync(ClassifyTypes? classifyType,string? searchText)
        {
            IEnumerable<VodVideoClassify> enumerable =await vodVideoClassifyRepository.QueryVodVideoClassifyAsync(classifyType,searchText);
            return enumerable;
        }
        /// <summary>
        /// 删除视频分类
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task RemoveVodVideoClassifyAsync(params Guid[] ids)
        {
            foreach (Guid id in ids)
            {
                await vodVideoClassifyRepository.RemoveVodVideoClassifyByIdAsync(id);
            }
        }
        /// <summary>
        /// 新增视频片段
        /// </summary>
        /// <param name="vodVideoPart"></param>
        /// <returns></returns>
        public async Task<(bool,string)> AddVodVideoPartAsync(Guid videoId,VodVideoPart vodVideoPart)
        {
            VodVideo? video=await vodVideoRepository.FindVodVideoByIdAsync(videoId);
            if (video == null) {
                return (false, "视频不存在");
            }
            vodVideoPart.Video = video;
            await vodVideoPartRepository.AddVodVideoPartAsync(vodVideoPart);
            return (true, "");
        }
        /// <summary>
        /// 更新视频片段
        /// </summary>
        /// <param name="vodVideoPart"></param>
        /// <returns></returns>
        public (bool,string) UpdateVodVideoPart(VodVideoPart vodVideoPart)
        {
            vodVideoPartRepository.UpdateVodVideoPart(vodVideoPart);
            return (true, "");
        }
        /// <summary>
        /// 查询视频片段
        /// </summary>
        /// <param name="videoId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<VodVideoPart>> QueryVodVideoPartAsync(Guid videoId,string? searchText)
        {
            return await vodVideoPartRepository.QueryVodVideoPartAsync(videoId,searchText);
        }

        public async Task<VodVideoPart?> GetVodVideoPartAsync(Guid id)
        {
            VodVideoPart? part=await vodVideoPartRepository.GetVodVideoPartByIdAsync(id);
            return part;
        }

        /// <summary>
        /// 删除视频片段
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task RemoveVodVideoPartAsync(Guid[] ids)
        {
            foreach (Guid id in ids) {
                await vodVideoPartRepository.RemoveVodVideoPartByIdAsync(id);
            }
        }
        /// <summary>
        /// 新增评论
        /// </summary>
        /// <param name="vodVideoComment"></param>
        /// <returns></returns>
        public async Task<(bool,string)> AddVodVideoCommentAsync(Guid videoId,VodVideoComment vodVideoComment, Guid? rootVodVideoCommentId=null)
        {
            VodVideo? vodVideo=await vodVideoRepository.FindVodVideoByIdAsync(videoId);
            if (vodVideo == null)
            {
                return (false, "不存在该视频");
            }
            if (rootVodVideoCommentId != null)
            {
                VodVideoComment? rootVodVideoComment =await vodVideoCommentRepository.FindVodVideoCommentByIdAsync(rootVodVideoCommentId.GetValueOrDefault());
                if(rootVodVideoComment == null)
                {
                    return (false, "不存在该根评论");
                }
                vodVideoComment.RootVideoComment = rootVodVideoComment;
            }
            vodVideoComment.Video=vodVideo;
            await vodVideoCommentRepository.AddVodVideoCommentAsync(vodVideoComment);
            return (true, "");
        }
    }
}
