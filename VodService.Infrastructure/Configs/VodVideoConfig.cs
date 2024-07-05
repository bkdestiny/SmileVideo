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
            builder.HasKey(e => e.Id).IsClustered(false);
            builder.Property(v => v.VideoName).HasMaxLength(200).IsUnicode().IsRequired();
            builder.Property(v => v.Scriptwriter).IsUnicode();
            builder.Property(v=>v.Profile).HasMaxLength(500).IsUnicode();
            builder.Property(v=>v.Performers).HasMaxLength(500).IsUnicode();
            builder.Property(v=>v.Description).HasMaxLength(500).IsUnicode();
            builder.Property(v => v.VideoStatus).HasDefaultValue(VideoStatuses.Public);
            //配置视频表与视频片段表的一对多关系
            builder.HasMany<VodVideoPart>(v => v.VideoParts).WithOne(p => p.Video);
            //配置视频表与视频分类表的多对多关系
            builder.HasMany<VodVideoClassify>(v => v.VideoClassifies).WithMany(c => c.Videos).UsingEntity(r=>r.ToTable("VodVideoClassifyRelation"));
            //配置视频表与视频评论表的一对多关系
            builder.HasMany<VodVideoComment>(v=>v.VideoComments).WithOne(c=>c.Video);
        }
    }
}
