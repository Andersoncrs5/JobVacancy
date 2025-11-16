namespace JobVacancy.API.models.entities;

public class FollowerUserRelationshipEnterpriseEntity: BaseEntity
{
    public required string UserId { get; set; }
    public UserEntity? User { get; set; }
    
    public required string EnterpriseId { get; set; }
    public EnterpriseEntity? Enterprise { get; set; }
    
    public bool WishReceiveNotifyByNewPost { get; set; } = true; 
    public bool WishReceiveNotifyByNewVacancy { get; set; } = true; 
    public bool WishReceiveNotifyByNewComment { get; set; } = true;
    public bool WishReceiveNotifyByNewInteraction { get; set; } = false;
}