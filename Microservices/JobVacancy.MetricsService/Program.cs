using Confluent.Kafka;
using JobVacancy.API.Configs.kafka.Classes;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.Configure<KafkaConfig>(
    builder.Configuration.GetSection("KafkaConfig"));

var kafkaConfigSection = builder.Configuration.GetSection("KafkaConfig");

var producerConfig = new ProducerConfig();
kafkaConfigSection.Bind(producerConfig);
producerConfig.BrokerAddressFamily = BrokerAddressFamily.V4;
builder.Services.AddSingleton(producerConfig);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();

public partial class Program { }