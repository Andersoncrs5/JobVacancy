using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.entities;

public class FollowerUserRelationshipEnterpriseEntity: BaseEntity
{
    [MaxLength(450)] public required string UserId { get; set; }
    public UserEntity? User { get; set; }
    
    [MaxLength(450)] public required string EnterpriseId { get; set; }
    public EnterpriseEntity? Enterprise { get; set; }
    
    public bool WishReceiveNotifyByNewPost { get; set; } = true; 
    public bool WishReceiveNotifyByNewVacancy { get; set; } = true; 
    public bool WishReceiveNotifyByNewComment { get; set; } = true;
    public bool WishReceiveNotifyByNewInteraction { get; set; } = false;
}