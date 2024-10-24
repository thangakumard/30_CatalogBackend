using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Services;
using Polly;
using Polly.Retry; 
using SharedLibrary;

namespace Orders.Application.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection services,
            IConfiguration config)
        {
            //Register HttpClient
            //Create Dependency Injection

            services.AddHttpClient<IOrderService, OrderService>(option =>
            {
                option.BaseAddress = new Uri(config["ApiGateway:BaseAddress"]!);
                option.Timeout = TimeSpan.FromSeconds(1);
            });

            //Retry
            var retryStrategyOptions = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                BackoffType = DelayBackoffType.Constant,
                UseJitter = true,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMicroseconds(500),
                OnRetry = arg =>
                {
                    string message = $"On retry, attempt : {arg.AttemptNumber}.Outcome :{arg.Outcome}";
                    Logger.LogToConsole(message);
                    Logger.LogToConsole(message);
                    return ValueTask.CompletedTask;
                }
            };

            //Use Retry 
            services.AddResiliencePipeline("orders-retry-pipeline", builder =>
            {
                builder.AddRetry(retryStrategyOptions);
            });

            return services;
        }
    }
}
