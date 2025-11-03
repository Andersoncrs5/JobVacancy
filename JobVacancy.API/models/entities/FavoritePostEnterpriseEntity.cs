namespace JobVacancy.API.models.entities;

public class FavoritePostEnterpriseEntity: BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public virtual UserEntity? User { get; set; }
    
    public string PostEnterpriseId { get; set; } = string.Empty;
    public virtual PostEnterpriseEntity? PostEnterprise { get; set; } 
}