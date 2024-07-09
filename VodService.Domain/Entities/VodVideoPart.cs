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
        public Guid? PartFile {  get; set; }

        /// <summary>
        /// 上映时间
        /// </summary>
        public DateTime ReleaseTime { get; set; }

        /// <summary>
        /// 片段状态
        /// </summary>
        public PartStatuses PartStatus { get; set; } = PartStatuses.Public;

        /// <summary>
        /// 排序
        /// </summary>
        public int SortIndex { get; set ; }

        private VodVideoPart() { }
        public VodVideoPart(Guid id,string partName, Guid? partFile, DateTime releaseTime, PartStatuses partStatuses, int sortIndex=999)
        {
            Id = id == Guid.Empty ? Guid.NewGuid() : id;
            PartName = partName;
            PartFile = partFile;
            ReleaseTime = releaseTime;
            PartStatus = partStatuses;
            SortIndex = sortIndex;
        }
        public enum PartStatuses
        {
            Public,Private
        }
    }
}
