using Common.EFcore;
using LiveService.Domain.Entites;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveService.Infrastructure
{
    public class LiveDbContext : BaseDbContext
    {
        public DbSet<LiveRoom> LiveRooms { get; set; }
        public LiveDbContext(DbContextOptions options, IMediator? mediator) : base(options, mediator)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
            modelBuilder.EnableSoftDeletionGlobalFilter();

        }
    }
}
