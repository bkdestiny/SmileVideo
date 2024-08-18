using LiveService.Domain.Entites;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveService.Infrastructure.Configs
{
    internal class LiveRoomConfig : IEntityTypeConfiguration<LiveRoom>
    {
        public void Configure(EntityTypeBuilder<LiveRoom> builder)
        {
            builder.ToTable("LiveRoom");
            builder.HasKey(x => x.Id).IsClustered(false);
            builder.Property(x => x.UserId).IsRequired();
            builder.Property(x => x.Title).HasMaxLength(20);
        }
    }
}
