using Common.EFcore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VodService.Domain.Entities
{
    public class VodVideoPart : BaseEntity, IHasSortIndex
    {
        /// <summary>
        /// 视频片段名称
        /// </summary>
        public string PartName { get; set; }

        /// <summary>
        /// 与视频表多对一
        /// </summary>
        public VodVideo Video { get; set; }

        /// <summary>
        /// 片段文件ID
        /// </summary>
        public Guid PartFile {  get; set; }

        /// <summary>
        /// 上映时间
        /// </summary>
        public DateTime ReleaseTime { get; set; }

        /// <summary>
        /// 是否隐藏该片段
        /// </summary>
        public bool IsHidden { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortIndex { get; set ; }

        public VodVideoPart(string partName, VodVideo video, Guid partFile, DateTime releaseTime, bool isHidden=false, int sortIndex=999)
        {
            PartName = partName;
            Video = video;
            PartFile = partFile;
            ReleaseTime = releaseTime;
            IsHidden = isHidden;
            SortIndex = sortIndex;
        }
    }
}
