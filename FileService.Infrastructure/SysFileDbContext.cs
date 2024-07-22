using Common.EFcore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FileService.Domain.Entites;
namespace FileService.Infrastructure
{
    public class SysFileDbContext : BaseDbContext
    {
        public DbSet<SysFile> SysFiles {  get; private set; } 
        public SysFileDbContext(DbContextOptions<SysFileDbContext> options, IMediator? mediator) : base(options, mediator)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        }
    }
}
