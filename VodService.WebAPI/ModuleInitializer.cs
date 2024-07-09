using Common;
using VodService.Domain.DomainServices;
using VodService.Domain.IRepositories;
using VodService.Infrastructure.Repositories;

namespace VodService.WebAPI
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IVodVideoRepository,VodVideoRepository>();
            services.AddScoped<IVodVideoClassifyRepository,VodVideoClassifyRepository>();
            services.AddScoped<IVodVideoPartRepository,VodVideoPartRepository>();
            services.AddScoped<VodDomainService>();
        }
    }
}
