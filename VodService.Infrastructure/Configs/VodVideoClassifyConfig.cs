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
    public class VodVideoClassifyConfig : IEntityTypeConfiguration<VodVideoClassify>
    {
        public void Configure(EntityTypeBuilder<VodVideoClassify> builder)
        {
            builder.ToTable("VodVideoClassify");
            builder.HasKey(e => e.Id).IsClustered(false);//对于Guid主键，不要建聚集索引，否则插入性能很差
            builder.Property(c => c.ClassifyName).IsUnicode().IsRequired();
        }
    }
}
