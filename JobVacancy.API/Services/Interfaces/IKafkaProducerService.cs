using JobVacancy.API.Configs.kafka.Classes;

namespace JobVacancy.API.Services.Interfaces;

public interface IKafkaProducerService
{
    Task MetricSend(MetricEvent message);
    void Dispose();
}