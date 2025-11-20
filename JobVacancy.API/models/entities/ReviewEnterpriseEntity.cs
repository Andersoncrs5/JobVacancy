using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Base;

namespace JobVacancy.API.models.entities;

public class ReviewEnterpriseEntity: ReviewBase
{
    [MaxLength(450)] public required string PositionId { get; set; }
    public PositionEntity? Position { get; set; }
    
    [MaxLength(450)] public required string UserId { get; set; }
    public UserEntity? User { get; set; }
    
    [MaxLength(450)] public required string EnterpriseId { get; set; }
    public EnterpriseEntity? Enterprise { get; set; }
    
}