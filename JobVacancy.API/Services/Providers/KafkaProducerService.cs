using System.Text.Json;
using Confluent.Kafka;
using JobVacancy.API.Configs.kafka.Classes;
using JobVacancy.API.Configs.kafka.Enums;
using JobVacancy.API.models.entities;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Services.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class KafkaProducerService: IKafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<KafkaProducerService> _logger;
    private readonly IConfiguration _configuration;

    public KafkaProducerService(ILogger<KafkaProducerService> logger, IConfiguration configuration)
    {
        _configuration = configuration;
        var settings = configuration.GetSection("KafkaSettings");
        _logger = logger;
        
        var config = new ProducerConfig { BootstrapServers = "localhost:9092" };
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _logger.LogInformation("Kafka Producer Initialized successfully.");
    }
    
    public async Task ProduceAsyncw(string message)
    {
        string topic = "calculation-metric-topic";

        var dr = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
        _logger.LogInformation($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
    }

    public async Task MetricSend(string entityId, ActionEnum action, ReactionTargetEnum entity, object columns, object? data = null)
    {
        var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            try
            {
                var sendResult = producer
                    .ProduceAsync("calculation-metric-topic", new Message<Null, string> { Value = entityId })
                    .GetAwaiter()
                    .GetResult();

                _logger.LogInformation($"Mensagem '{sendResult.Value}' de '{sendResult.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }
    }
    
    private async Task ProduceAsync<T>(string topic, T message)
    {
        string jsonMessage = JsonSerializer.Serialize(message);
            
        _logger.LogDebug("Trying to send a message to the topic {Topic}: {Message}", topic, jsonMessage);

        try
        {
            var deliveryReport = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = jsonMessage });
                
            _logger.LogInformation("Message delivered to Kafka. Topic/Partition/Offset: {Topic}/{Partition}/{Offset}", 
                deliveryReport.Topic, deliveryReport.Partition, deliveryReport.Offset);
        }
        catch (ProduceException<Null, string> e)
        {
            _logger.LogError(e, "Message delivered to Kafka failed.: {ErrorReason}", e.Error.Reason);
        }
    }
    
    private void FireAndForgetProduce<T>(string topic, T message)
    {
        string jsonMessage = JsonSerializer.Serialize(message);
        var kafkaMessage = new Message<Null, string> { Value = jsonMessage };
            
        _logger.LogDebug("Tentando enviar mensagem (Fire-and-Forget) para o tópico {Topic}: {Message}", topic, jsonMessage);

        try
        {
            _producer.Produce(topic, kafkaMessage, deliveryReport =>
            {
                if (deliveryReport.Error.Code != ErrorCode.NoError)
                {
                    _logger.LogError("Falha na entrega da mensagem para o Kafka: {ErrorReason}", deliveryReport.Error.Reason);
                }
                else
                {
                    _logger.LogInformation("Mensagem entregue com sucesso. Topic/Partition/Offset: {Topic}/{Partition}/{Offset}", 
                        deliveryReport.Topic, deliveryReport.Partition, deliveryReport.Offset);
                }
            });
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Erro síncrono ao tentar iniciar a produção da mensagem Kafka.");
        }
    }
    
    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}