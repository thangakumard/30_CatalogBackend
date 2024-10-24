using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Interfaces;
using Orders.Infrastructure.Data;
using Orders.Infrastructure.Repositories;
using SharedLibrary.DependencyInjection;
using SharedLibrary.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services,
            IConfiguration config)
        {
            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, config, config["MySerilog:FileName"]!);

            //Add Dependency Injection
            services.AddScoped<IOrder, OrderRepository>();
            return services;
        }
        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
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
