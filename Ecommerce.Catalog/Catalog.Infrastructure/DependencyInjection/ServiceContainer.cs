using Catalog.Application.Interfaces;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharedLibrary.DependencyInjection;
using SharedLibrary.Middleware;

namespace Catalog.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        //Add database connectivity
        //Add Authentication scheme
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
            IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, config, config["MySerilog:FileName"]!);

            //Add Dependency Injection
            services.AddScoped<IProduct, ProductRepository>();
            return services; 
        }

        public  static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Registor middleware such as
            //global exception,
            //Listen only to Api gateway (block extenal calls)
            app.UseMiddleware<GlobalException>();
            //app.UseMiddleware<ListentoOnlyApiGateway>();
            //SharedServiceContainer.UseSharedPolicies(app);

            return app;

        }



        
    }
}
