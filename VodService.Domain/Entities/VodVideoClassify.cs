using Common.EFcore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VodService.Domain.Entities
{
    public class VodVideoClassify : BaseEntity, IHasSortIndex
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        public string ClassifyName { get; set; }

        /// <summary>
        /// 分类类型
        /// </summary>
        public ClassifyTypes ClassifyType { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortIndex { get; set; }
        /// <summary>
        /// 与视频表的多对多关系 
        /// </summary>
        public List<VodVideo> Videos { get; set; } = new List<VodVideo>();
        public VodVideoClassify(string classifyName, ClassifyTypes classifyType, int sortIndex = 999)
        {
            ClassifyName = classifyName;
            ClassifyType = classifyType;
            SortIndex = sortIndex;
        }

    }

    public enum ClassifyTypes
    {
        /// <summary>
        /// 类型
        /// </summary>
        Type,
        /// <summary>
        /// 风格
        /// </summary>
        Style,
        /// <summary>
        /// 地区
        /// </summary>
        Area
    }
}
