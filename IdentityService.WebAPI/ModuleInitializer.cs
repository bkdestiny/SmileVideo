using Common;
using IdentityService.Domain.IRepositories;
using IdentityService.Domain.DomainServices;
using IdentityService.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace IdentityService.WebAPI
{
    public class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IdentityDomainService>();
        }

    }
}
