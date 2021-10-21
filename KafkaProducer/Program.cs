using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace KafkaProducer
{
    public class Program
    {
        static async Task Main()
        {
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            logger.Information("Iniciando envio de mensagens com Kafka!");

            string bootstrapServers = "localhost:9092";
            string topic = "event-push-notification";

            int maxMessages = 4;

            try
            {
                for (int i = 0; i <= maxMessages; i++)
                {
                    var message = new MessageModel
                    {
                        UserId = Guid.NewGuid(),
                        Title = "Olá!",
                        Message = $"Estou te enviando uma notificação push... {DateTime.Now}"
                    };

                    var messageJson = JsonConvert.SerializeObject(message, new JsonSerializerSettings() 
                    { 
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });

                    var config = new ProducerConfig { BootstrapServers = bootstrapServers };

                    using var producer = new ProducerBuilder<Null, string>(config).Build();

                    var result = await producer.ProduceAsync(topic, new Message<Null, string> { Value = messageJson });

                    logger.Information($"Mensagem: { messageJson } | Status: { result.Status }");

                    await Task.Delay(2000);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Exceção: {ex.GetType().FullName} | Mensagem: {ex.Message}");
            }
        }
    }
}
