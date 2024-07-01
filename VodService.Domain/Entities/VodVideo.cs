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

        public List<VodVideoPart> VideoParts { get; set; }=new List<VodVideoPart>();

        /// <summary>
        /// 与视频分类表的多对多关系 
        /// </summary>
        public List<VodVideoClassify> VideoClassifies { get; set; }=new List<VodVideoClassify>();

        public DateTime CreateTime { get; private set; }

        public bool IsDeleted { get; private set; }

        public DateTime? DeleteTime { get; private set; }

        public void SoftDelete()
        {
            this.DeleteTime = DateTime.Now;
            this.IsDeleted = true;
        }
    }
}
