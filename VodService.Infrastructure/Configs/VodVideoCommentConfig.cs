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
    public class VodVideoCommentConfig : IEntityTypeConfiguration<VodVideoComment>
    {
        public void Configure(EntityTypeBuilder<VodVideoComment> builder)
        {
            builder.ToTable("VodVideoComment");
            builder.HasKey(e => e.Id).IsClustered(false);// 对于Guid主键，不要建聚集索引，否则插入性能很差
            builder.Property(c=>c.UserId).IsRequired();
            builder.Property(c => c.Content).IsUnicode().IsRequired();
            //根评论与子评论的一对多关系
            builder.HasOne<VodVideoComment>(c => c.RootVideoComment).WithMany(c => c.SubVideoComments);
        }
    }
}
