using Common;
using FileService.Domain.DomainServices;
using FileService.Domain.IRepositories;
using FileService.Infrastructure.Repositories;


namespace FileService.WebAPI
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<ISysFileRepository, SysFileRepository>();
            services.AddScoped<SysFileDomainService>();
        }

    }
}
