using System.Text.Json;
using Confluent.Kafka; 
using JobVacancy.API.Configs.kafka.Classes;
using Microsoft.Extensions.Options;

namespace JobVacancy.MetricsService.Services.Providers;

public class MetricConsumerService: BackgroundService
{
    private readonly IConfiguration _configuration;
    private readonly IConsumer<string, string> _consumer;
    private readonly string _topic;
    private readonly ILogger<MetricConsumerService> _logger;

    public MetricConsumerService(
        IOptions<KafkaConfig> kafkaConfig, 
        ILogger<MetricConsumerService> logger,
        IConfiguration configuration
        )
    {
        _logger = logger;
        _configuration = configuration;
        var config = kafkaConfig.Value.ConsumerConfig;
        _topic = _configuration["KafkaConfig:Topics:CalculationMetricTopic"] ?? throw new NullReferenceException();
        
        config.GroupId = "job-vacancy-metrics-group"; 
        config.AutoOffsetReset = AutoOffsetReset.Earliest; 
        config.EnableAutoCommit = false; 
        
        _consumer = new ConsumerBuilder<string, string>(config)
            .SetErrorHandler((_, e) => _logger.LogError($"Kafka Error: {e.Reason}"))
            .SetLogHandler((_, l) => _logger.LogInformation($"Kafka Log: {l.Message}"))
            .Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"Kafka Consumer Service iniciado, escutando o tópico: {_topic}");
        
        _consumer.Subscribe(_topic);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                if (consumeResult == null) continue;
                
                var metricEvent = JsonSerializer.Deserialize<MetricEvent>(consumeResult.Message.Value);

                if (metricEvent != null)
                {
                    await ProcessMessage(metricEvent);
                    
                    _consumer.Commit(consumeResult);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ConsumeException e)
            {
                _logger.LogError($"Consume Error ({_topic}): {e.Error.Reason}");
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"An unexpected error occurred during message processing.");
            }
        }
    }

    private async Task ProcessMessage(MetricEvent message)
    {
        _logger.LogInformation($"Mensagem recebida do Kafka: ID={message.Id}, Ação={message.Action}");
        
        
        
        await Task.Delay(1); 
    }

    public override void Dispose()
    {
        _consumer.Close(); 
        _consumer.Dispose();
        base.Dispose();
    }
}