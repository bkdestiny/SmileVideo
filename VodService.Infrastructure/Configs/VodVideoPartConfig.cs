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
    public class VodVideoPartConfig : IEntityTypeConfiguration<VodVideoPart>
    {
        public void Configure(EntityTypeBuilder<VodVideoPart> builder)
        {
            builder.ToTable("VodVideoPart");
            builder.Property(p => p.PartName).IsUnicode().IsRequired();
            builder.Property(p => p.Video).IsRequired();
        }
    }
}
