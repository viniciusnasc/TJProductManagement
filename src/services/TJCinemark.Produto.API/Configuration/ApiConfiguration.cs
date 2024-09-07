using Amazon.Runtime.Internal;

namespace TJ.ProductManagement.API.Configuration
{
    public static class ApiConfiguration
    {
        public static void AddApiConfiguration(this IServiceCollection services)
        {
            services.AddControllers();
        }

        public static void UseApiConfiguration(this IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
        }
    }
}
