using Common.EFcore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VodService.Domain.Entities;

namespace VodService.Infrastructure
{
    public class VodDbContext : BaseDbContext
    {
        public DbSet<VodVideo> VodVideos { get; set; }
        public DbSet<VodVideoPart> VodVideoParts { get; set; }
        public DbSet<VodVideoClassify> VodVideoClassifies { get; set; }
        public VodDbContext(DbContextOptions options, IMediator? mediator) : base(options, mediator)
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
