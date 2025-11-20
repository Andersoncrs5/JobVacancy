using JobVacancy.API.Configs.kafka.Enums;

namespace JobVacancy.API.Configs.kafka.Classes;

public class MetricEvent
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public required ActionEnum Action { get; set; }
    public required EntityEnum Entity { get; set; }
    public required string Column { get; set; } 
    public required string EntityId { get; set; }
    public object? Data { get; set; }
    public object? Metadata { get; set; }
    
    public MetricEvent() { }
}