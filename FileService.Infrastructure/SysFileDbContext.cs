using Common.EFcore;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FileService.Infrastructure
{
    public class SysFileDbContext : BaseDbContext
    {
        public SysFileDbContext(DbContextOptions options, IMediator? mediator) : base(options, mediator)
        {

        }
    }
}
