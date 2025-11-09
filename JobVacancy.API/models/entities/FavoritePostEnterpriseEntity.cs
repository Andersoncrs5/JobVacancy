namespace JobVacancy.API.models.entities;

public class FavoritePostEnterpriseEntity: BaseEntity
{
    public string UserId { get; set; } = string.Empty;
    public UserEntity? User { get; set; }
    
    public string PostEnterpriseId { get; set; } = string.Empty;
    public PostEnterpriseEntity? PostEnterprise { get; set; } 
}