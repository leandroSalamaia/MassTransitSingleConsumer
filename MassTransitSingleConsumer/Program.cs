using MassTransit;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace Mjolnir.RabbitMq
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var queues = new string[] { "Message001", "Message002", "Message003"};

            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddMassTransit(x =>
                    {
                        x.AddConsumer<MessageConsumer>();

                        x.UsingRabbitMq((cxt, cfg) =>
                        {
                            cfg.Host("localhost", "/", h =>
                            {
                                h.Username("guest");
                                h.Password("guest");
                            });

                            cfg.ClearMessageDeserializers();
                            cfg.UseRawJsonSerializer();
                            
                            queues.ToList().ForEach(queue =>
                            {
                                cfg.ReceiveEndpoint(queue, e =>
                                {
                                    e.UseConcurrencyLimit(1);
                                    e.ConfigureConsumer<MessageConsumer>(cxt);
                                });
                            });
                        });
                    });
                });
        }
    }
}