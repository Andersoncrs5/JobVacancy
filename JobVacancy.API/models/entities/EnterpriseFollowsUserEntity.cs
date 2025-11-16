namespace JobVacancy.API.models.entities;

public class EnterpriseFollowsUserEntity: BaseEntity
{
    public required string EnterpriseId { get; set; }
    public EnterpriseEntity? Enterprise { get; set; }
    
    public required string UserId { get; set; }
    public UserEntity? User { get; set; }

    public bool WishReceiveNotifyByNewPost { get; set; } = true; 
    public bool WishReceiveNotifyByNewEndorsement { get; set; } = true; 
    public bool WishReceiveNotifyByProfileUpdate { get; set; } = true;
    public bool WishReceiveNotifyByNewInteraction { get; set; } = false;
}