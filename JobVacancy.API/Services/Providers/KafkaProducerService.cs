using System.Text.Json;
using Confluent.Kafka;
using JobVacancy.API.Configs.kafka.Classes;
using JobVacancy.API.Services.Interfaces;

namespace JobVacancy.API.Services.Providers;

public class KafkaProducerService: IKafkaProducerService
{
    private readonly IProducer<Null, string> _producer;
    private readonly ILogger<KafkaProducerService> _logger;
    private readonly IConfiguration _configuration;

    public KafkaProducerService(ProducerConfig config, ILogger<KafkaProducerService> logger, IConfiguration configuration)
    {
        _logger = logger;
        _producer = new ProducerBuilder<Null, string>(config).Build();
        _logger.LogInformation("Kafka Producer Initialized successfully.");
        _configuration = configuration;
    }

    public async Task MetricSend(MetricEvent message)
    {
        string topic = _configuration["KafkaConfig:Topics:CalculationMetricTopic"] ?? throw new NullReferenceException();
        
        await ProduceAsync(topic, message);
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
    
    public void Dispose()
    {
        _producer.Flush(TimeSpan.FromSeconds(10));
        _producer.Dispose();
    }
}