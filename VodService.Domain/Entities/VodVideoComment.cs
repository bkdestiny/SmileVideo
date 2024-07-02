using Common.EFcore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VodService.Domain.Entities
{
    public class VodVideoComment : BaseEntity, IHasCreateTime
    {
        /// <summary>
        /// 所属视频
        /// </summary>
        public VodVideo Video {  get; set; }

        /// <summary>
        /// 评论用户Id
        /// </summary>
        public Guid UserId { get; set; }
        /// <summary>
        /// 被回复用户Id
        /// </summary>
        public Guid? RespondentUserId {  get; set; }
        /// <summary>
        /// 根评论
        /// </summary>
        public VodVideoComment? RootVideoComment { get; set; }
        /// <summary>
        /// 子评论
        /// </summary>
        public List<VodVideoComment> SubVideoComments {  get; set; }=new List<VodVideoComment>();
        /// <summary>
        /// 评论内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime CreateTime {  get;private set; }
        /// <summary>
        /// 评论类型 根评论 or 子评论
        /// </summary>
        public VodVideoCommentTypes VideoCommentTypes { get; set; }

        /// <summary>
        /// 点赞数量
        /// </summary>
        public int LikesCount { get; private set; } = 0;

        private VodVideoComment()
        {
            
        }
        /// <summary>
        /// 创建根评论
        /// </summary>
        /// <param name="video"></param>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public VodVideoComment CreateRootVideoComment(VodVideo video, Guid userId,  string content)
        {
            VodVideoComment instance= new VodVideoComment();
            instance.Video = video;
            instance.UserId = userId;
            instance.Content = content;
            instance.VideoCommentTypes = VodVideoCommentTypes.Root;
            instance.CreateTime=DateTime.Now;
            return instance;
        }
        /// <summary>
        /// 创建子评论
        /// </summary>
        /// <param name="video"></param>
        /// <param name="userId"></param>
        /// <param name="rootVideoComment"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public VodVideoComment CreateSubVideoComment(VodVideo video, Guid userId,VodVideoComment rootVideoComment, string content) {
            VodVideoComment instance = new VodVideoComment();
            instance.Video = video;
            instance.UserId = userId;
            instance.Content = content;
            instance.RootVideoComment=rootVideoComment;
            instance.VideoCommentTypes = VodVideoCommentTypes.Sub;
            instance.CreateTime = DateTime.Now;
            return instance;
        }
        /// <summary>
        /// 创建回复子评论
        /// </summary>
        /// <param name="video"></param>
        /// <param name="userId"></param>
        /// <param name="respondentUserId"></param>
        /// <param name="rootVideoComment"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public VodVideoComment CreateAnswerSubVideoComment(VodVideo video,Guid userId,Guid respondentUserId,VodVideoComment rootVideoComment,string content)
        {
            VodVideoComment instance = new VodVideoComment();
            instance.Video = video;
            instance.UserId = userId;
            instance.RespondentUserId = respondentUserId;
            instance.Content = content;
            instance.RootVideoComment = rootVideoComment;
            instance.VideoCommentTypes = VodVideoCommentTypes.Sub;
            instance.CreateTime = DateTime.Now;
            return instance;
        }
    }

    public enum VodVideoCommentTypes
    {
        Root,Sub
    }
}
