// using System.Reflection;
// using System.Threading.Tasks;
// using Microsoft.Extensions.Hosting;
// using MassTransit;
// using Microsoft.Extensions.DependencyInjection;
// using WorkerSample.Workers;
// using Microsoft.Extensions.Configuration;
// using WorkerSample.Modules;
// using Microsoft.Extensions.Options;
// using WorkerSample.Consumers.WorkerSample.Consumers;
// using WorkerSample.Contracts;

// namespace WorkerSample
// {
//     public class Program
//     {
//         public static async Task Main(string[] args)
//         {
//             await CreateHostBuilder(args).Build().RunAsync();
//         }

//         public static IHostBuilder CreateHostBuilder(string[] args) =>
//             Host.CreateDefaultBuilder(args)
//                 .ConfigureAppConfiguration((context, config) =>
//                 {
//                     config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//                     if (context.HostingEnvironment.IsDevelopment())
//                     {
//                         config.AddUserSecrets<Program>();
//                     }
//                 })
//                 .ConfigureServices((hostContext, services) =>
//                 {
//                     services.Configure<AwsOptions>(hostContext.Configuration.GetSection("AWS"));

//                     services.AddMassTransit(x =>
//                     {
//                         x.SetKebabCaseEndpointNameFormatter();

//                         // By default, sagas are in-memory, but should be changed to a durable
//                         // saga repository.
//                         x.SetInMemorySagaRepositoryProvider();

//                         var entryAssembly = Assembly.GetEntryAssembly();

//                         x.AddConsumers(entryAssembly);
//                         x.AddSagaStateMachines(entryAssembly);
//                         x.AddSagas(entryAssembly);
//                         x.AddActivities(entryAssembly);

//                         x.UsingAmazonSqs((context, cfg) =>
//                         {
//                             var awsOptions = context.GetRequiredService<IOptions<AwsOptions>>().Value;

//                             cfg.Host(awsOptions.Region, h =>
//                             {
//                                 h.AccessKey(awsOptions.AccessKey);
//                                 h.SecretKey(awsOptions.SecretKey);
//                             });

//                             // Disable the automatic topic creation and consume topology
//                             // cfg.ConfigureCo
//                             // cfg.PublishFaults = false;

//                             // Use pre-existing topic names (manually configured)
//                             // cfg.Message<YourMessageType>(c =>
//                             // {
//                             //     c.SetEntityName("your-preexisting-topic-name"); // Specify the existing topic name
//                             // });

//                             // Example of consuming messages from existing queues
//                             // cfg.ReceiveEndpoint("your-existing-queue-name", e =>
//                             // {
//                             //     e.ConfigureConsumer<YourConsumer>(context);
//                             // });

//                             // Comment this out to prevent mt from trying 
//                             // to automatically create the SQS queue and SNS topic
//                             // cfg.ConfigureEndpoints(context);

//                             // Specify the queue name directly
//                             // hello 
//                             cfg.ReceiveEndpoint(awsOptions.SqsQueueName, e =>
//                             {
//                                 // disable the default topic binding
//                                 e.ConfigureConsumeTopology = false;
//                                 e.PublishFaults = false;

//                                 // e.PublishFaults = false;

//                                 // Specify the topic name directly
//                                 // WorkerSample_Contracts-Hello
//                                 // e.Subscribe(awsOptions.SnsTopicName, s =>
//                                 // {
//                                 //     s.TopicAttributes["DisplayName"] = awsOptions.SnsTopicName;
//                                 // });


//                                 // e.ConfigurePublish(p =>
//                                 // {
//                                 //     p.
//                                 //     p.SMessage<Hello>(m =>
//                                 //     {
//                                 //         m.SetEntityName(awsOptions.SnsTopicName);
//                                 //     });
//                                 // });


//                                 // e.ConfigureConsumer<HelloConsumer>(context);
//                                 cfg.Message<Hello>(c =>
//                                 {
//                                     c.SetEntityName(awsOptions.SnsTopicName);
//                                 });
//                             });

//                             // cfg.ConfigureSend(p =>
//                             // {
//                             //     p.Message<Hello>(m =>
//                             //     {
//                             //         m.SetEntityName(awsOptions.SnsTopicName);
//                             //     });
//                             // });
//                         });

//                     });

//                     services.AddHostedService<Worker>();
//                 });
//     }
// }
