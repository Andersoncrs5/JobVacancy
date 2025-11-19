using JobVacancy.API.models.entities.Base;

namespace JobVacancy.API.models.entities;

public class ReviewUserEntity : ReviewBase
{
    public bool? Recommendation { get; set; }
    
    public required string ActorId { get; set; }
    public UserEntity? Actor { get; set; }

    public required string TargetUserId { get; set; }
    public UserEntity? TargetUser { get; set; }
}