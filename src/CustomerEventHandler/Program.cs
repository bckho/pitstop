using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pitstop.Infrastructure.Messaging.Configuration;
using Serilog;

namespace CustomerEventHandler
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }
        
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.UseRabbitMQMessageHandler(hostContext.Configuration);

                    services.AddHostedService<CustomerManager>();
                })
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .UseConsoleLifetime();
        
            return hostBuilder;
        }
    }
}
