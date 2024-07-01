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
            builder.Property(c => c.ClassifyName).IsUnicode().IsRequired();
        }
    }
}
