using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class UserActivityEvent : BaseEntity
{
    public required string UserId { get; set; }
    public UserEntity? User { get; set; }
    
    public required UserEventTypeEnum Type { get; set; }

    public string? RelatedEntityId { get; set; }
}
