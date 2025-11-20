using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.entities;

public class FavoritePostEnterpriseEntity: BaseEntity
{
    [MaxLength(450)] public required string UserId { get; set; }
    public UserEntity? User { get; set; }
    
    [MaxLength(450)] public required string PostEnterpriseId { get; set; }
    public PostEnterpriseEntity? PostEnterprise { get; set; } 
}