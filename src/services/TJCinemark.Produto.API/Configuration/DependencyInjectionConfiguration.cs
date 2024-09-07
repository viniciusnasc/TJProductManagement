using TJ.ProductManagement.Data.Repository;
using TJ.ProductManagement.Domain.ErrorNotificator;
using TJ.ProductManagement.Domain.Interfaces.Repositories;
using TJ.ProductManagement.Domain.Interfaces.Services;
using TJ.ProductManagement.Services;

namespace TJ.ProductManagement.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            services.AddRepositoryDependency();
            services.AddServiceDependency();
            return services;
        }

        public static IServiceCollection AddRepositoryDependency(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            return services;
        }

        public static IServiceCollection AddServiceDependency(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<INotificator, Notificator>();
            return services;
        }
    }
}
