using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SharedLibrary.Middleware;

namespace SharedLibrary.DependencyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services, 
            IConfiguration config, string fileName) where TContext: DbContext {

            //services.AddDbContext<TContext>(option => option.UseSqlServer(
            //    config.GetConnectionString("eCommerceConnection"), 
            //    sqlserveroption => sqlserveroption.EnableRetryOnFailure()
            //    ));

            services.AddDbContext<TContext>(option => option.UseSqlite(
                config.GetConnectionString("eCommerceConnection")
                ));

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.Debug()
                .CreateLogger();

            JWTAuthenticationScheme.AddJWTAuthenticationScheme(services, config);
            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app)
        {
            app.UseMiddleware<GlobalException>();

            app.UseMiddleware<ListentoOnlyApiGateway>();

            return app;

        }
    }
}
