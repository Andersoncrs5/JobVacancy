using JobVacancy.API.models.dtos.PostEnterprise;
using JobVacancy.API.models.dtos.Users;

namespace JobVacancy.API.models.dtos.FavoritePostEnterprise;

public class FavoritePostEnterpriseDto: BaseDto
{
    public string UserId { get; set; } = string.Empty;
    public UserDto? User { get; set; }
    
    public string PostEnterpriseId { get; set; } = string.Empty;
    public PostEnterpriseDto? PostEnterprise { get; set; }
    
}