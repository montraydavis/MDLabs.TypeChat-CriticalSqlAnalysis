using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoGen.Core;
using MDLabs.CriticalSqlAnalysis.Agents;
using MDLabs.CriticalSqlAnalysis.Agents.Factories;
using MDLabs.CriticalSqlAnalysis.Handlers;
using MDLabs.CriticalSqlAnalysis.Handlers.Requests;
using MDLabs.CriticalSqlAnalysis.Loaders;
using MDLabs.CriticalSqlAnalysis.Models.Options;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MDLabs.CriticalSqlAnalysis;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    /// <summary>
    /// Creates the host builder for the application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    /// <returns>The host builder for the application.</returns>
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    // Load configuration from appsettings.json
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureServices((hostContext, services) =>
                {
                    // Load configuration from appsettings.json
                    var configuration = hostContext.Configuration;

                    // Configure options
                    var workerOptions = configuration.GetSection("Worker");
                    var agentProcessingOptions = configuration.GetSection("AgentProcessingOptions");
                    var openAIOptions = configuration.GetSection("OpenAIOptions");

                    services.Configure<WorkerOptions>(workerOptions);
                    services.Configure<AgentProcessingOptions>(agentProcessingOptions);
                    services.Configure<OpenAIOptions>(openAIOptions);

                    // Register services
                    services.AddMediatR(cfg =>
                        cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

                    services.AddHostedService<Worker>();
                })
                .ConfigureContainer<ContainerBuilder>(builder =>
                {
                    builder.RegisterType<ProjectFileLoader>()
                        .AsSelf()
                        .AsImplementedInterfaces()
                        .SingleInstance();


                    builder.RegisterType<ProjectFileHandler>()
                        .AsSelf()
                        .AsImplementedInterfaces()
                        .SingleInstance();

                    builder.RegisterType<AgentFactory>()
                        .AsSelf()
                        .AsImplementedInterfaces()
                        .SingleInstance();

                    builder.RegisterType<AnalysisAgent>()
                        .AsSelf()
                        .AsImplementedInterfaces();

                    builder.RegisterType<DocumentationHandler>()
                        .AsSelf()
                        .AsImplementedInterfaces()
                        .SingleInstance();
                });
}
