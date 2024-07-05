using Common.EFcore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VodService.Domain.Entities
{
    public class VodVideo : BaseEntity, IHasCreateTime, IHasDeleteTime, ISoftDelete
    {
        /// <summary>
        /// 视频名称
        /// </summary>
        public string VideoName { get; set; }

        /// <summary>
        /// 封面图片
        /// </summary>
        public Guid? CoverFile {  get; set; }


        public List<VodVideoPart> VideoParts { get; set; }=new List<VodVideoPart>();

        /// <summary>
        /// 与视频分类表的多对多关系 
        /// </summary>
        public List<VodVideoClassify> VideoClassifies { get; set; }=new List<VodVideoClassify>();

        /// <summary>
        /// 与视频评论的一对多关系
        /// </summary>
        public List<VodVideoComment> VideoComments { get; set; }=new List<VodVideoComment>();
        /// <summary>
        /// 出演人员
        /// </summary>
        public string? Performers {  get; set; } 
        /// <summary>
        /// 导演
        /// </summary>
        public string? Director {  get; set; }
        /// <summary>
        /// 编剧
        /// </summary>
        public string? Scriptwriter { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string? Profile { get; set; }

        public DateTime CreateTime { get; private set; }

        public bool IsDeleted { get; private set; }

        public DateTime? DeleteTime { get; private set; }
        /// <summary>
        /// 是否公开该视频
        /// </summary>
        public VideoStatuses VideoStatus { get; set; }
        public void SoftDelete()
        {
            this.DeleteTime = DateTime.Now;
            this.IsDeleted = true;
        }
        private VodVideo() { }
        public VodVideo(Guid id,string videoName, Guid? coverFile, string? performers, string? director, string? scriptwriter, string? description, string? profile, VideoStatuses videoStatus = VideoStatuses.Public)
        {
            Id=id==Guid.Empty? Guid.NewGuid():id;
            VideoName = videoName;
            CoverFile = coverFile;
            Performers = performers;
            Director = director;
            Scriptwriter = scriptwriter;
            Description = description;
            Profile = profile;
            VideoStatus = videoStatus;
            CreateTime = DateTime.Now;
        }
    }
    public enum VideoStatuses
    {
        Public,Private
    }
}
