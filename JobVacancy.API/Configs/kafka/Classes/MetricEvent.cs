using JobVacancy.API.Configs.kafka.Enums;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Configs.kafka.Classes;

public class MetricEvent
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public required ActionEnum Action { get; set; }
    public required ReactionTargetEnum Entity { get; set; }
    public required int Column { get; set; } 
    public required string EntityId { get; set; }
    public object? Data { get; set; }
    public object? Metadata { get; set; }
    
    public MetricEvent() { }
}