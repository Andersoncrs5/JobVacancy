

namespace JobVacancy.API.Configs.kafka.Classes;

public class KafkaConfig
{
    public string? BootstrapServers { get; set; }
    public string? GroupId { get; set; }

    public TopicNames? Topics { get; set; }
}
