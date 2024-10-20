namespace WorkerSample.Consumers
{
    using global::WorkerSample.Consumers.WorkerSample.Consumers;

    using MassTransit;


    public class HelloConsumerDefinition :
        ConsumerDefinition<HelloConsumer>
    {
        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<HelloConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));
            endpointConfigurator.UseInMemoryOutbox();
        }

        // protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator, IConsumerConfigurator<Consumer> consumerConfigurator, IRegistrationContext context)
        // {
        //     endpointConfigurator.UseMessageRetry(r => r.Intervals(500, 1000));

        //     endpointConfigurator.UseInMemoryOutbox(context);
        // }
    }
}