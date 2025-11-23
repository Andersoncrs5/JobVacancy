using JobVacancy.API.Configs.kafka.Classes;
using JobVacancy.API.Configs.kafka.Enums;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Services.Interfaces;

public interface IKafkaProducerService
{
    Task MetricSend(string entityId, ActionEnum action, ReactionTargetEnum entity, object columns, object? data = null);
    void Dispose();
}