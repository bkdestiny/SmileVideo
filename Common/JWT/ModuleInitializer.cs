using Microsoft.Extensions.DependencyInjection;


namespace Common.JWT
{
    class ModuleInitializer : IModuleInitializer
    {
        public void Initialize(IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
