using MassTransit;
using ConsumerModels;

namespace Mjolnir.RabbitMq;

public class MessageConsumer: IConsumer<Message>
{
    private IConsumer<Message> _consumerImplementation;
    public Task Consume(ConsumeContext<Message> context)
    {
        Console.WriteLine(context.Message.Body);
        Thread.Sleep(5000);
        return Task.CompletedTask;
    }
}