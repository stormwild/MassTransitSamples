using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using WorkerSample.Workers;
using Microsoft.Extensions.Configuration;
using WorkerSample.Modules;
using Microsoft.Extensions.Options;
using WorkerSample.Consumers.WorkerSample.Consumers;
using WorkerSample.Contracts;

namespace WorkerSample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    if (context.HostingEnvironment.IsDevelopment())
                    {
                        config.AddUserSecrets<Program>();
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<AwsOptions>(hostContext.Configuration.GetSection("AWS"));

                    services.AddMassTransit(x =>
                    {
                        x.SetKebabCaseEndpointNameFormatter();

                        // By default, sagas are in-memory, but should be changed to a durable
                        // saga repository.
                        x.SetInMemorySagaRepositoryProvider();

                        var entryAssembly = Assembly.GetEntryAssembly();

                        x.AddConsumers(entryAssembly);
                        x.AddSagaStateMachines(entryAssembly);
                        x.AddSagas(entryAssembly);
                        x.AddActivities(entryAssembly);

                        x.UsingAmazonSqs((context, cfg) =>
                        {
                            var awsOptions = context.GetRequiredService<IOptions<AwsOptions>>().Value;

                            cfg.Host(awsOptions.Region, h =>
                            {
                                h.AccessKey(awsOptions.AccessKey);
                                h.SecretKey(awsOptions.SecretKey);
                            });

                            // Comment this out to prevent mt from trying 
                            // to automatically create the SQS queue and SNS topic
                            // cfg.ConfigureEndpoints(context);

                            // Specify the queue name directly
                            // hello 
                            cfg.ReceiveEndpoint(awsOptions.SqsQueueName, e =>
                            {
                                // disable the default topic binding
                                e.ConfigureConsumeTopology = false;
                                e.PublishFaults = false;
                            });

                        });

                    });

                    services.AddHostedService<Worker>();
                });
    }
}
