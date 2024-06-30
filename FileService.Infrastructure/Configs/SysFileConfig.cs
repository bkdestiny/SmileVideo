using FileService.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure.Configs
{
    class SysFileConfig : IEntityTypeConfiguration<SysFile>
    {
        public void Configure(EntityTypeBuilder<SysFile> builder)
        {
            builder.ToTable("SysFile");
            builder.HasKey(e => e.Id);
            builder.Property(e=>e.FileName).IsUnicode().HasMaxLength(1024);
            builder.Property(e=>e.FileSHA256Hash).IsUnicode().HasMaxLength(64);
            builder.HasIndex(e => new { e.FileSHA256Hash,e.FileSize});//复合索引
        }
    }
}
