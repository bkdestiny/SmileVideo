using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VodService.Domain.Entities;

namespace VodService.Infrastructure.Configs
{
    public class VodVideoConfig : IEntityTypeConfiguration<VodVideo>
    {
        public void Configure(EntityTypeBuilder<VodVideo> builder)
        {
            builder.ToTable("VodVideo");
            builder.Property(v => v.VideoName).IsUnicode().IsRequired();
            //配置视频表与视频片段表的一对多关系
            builder.HasMany<VodVideoPart>(v => v.VideoParts).WithOne(p => p.Video);
            //配置视频表与视频分类表的多对多关系
            builder.HasMany<VodVideoClassify>(v => v.VideoClassifies).WithMany(c => c.Videos).UsingEntity(r=>r.ToTable("VodVideoClassifyRelation"));
        }
    }
}
