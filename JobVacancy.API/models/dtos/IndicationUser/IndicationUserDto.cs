using JobVacancy.API.models.dtos.Users;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.IndicationUser;

public class IndicationUserDto: BaseDto
{
    public string EndorserId { get; set; } = string.Empty;
    public UserDto? Endorser { get; set; }

    public string EndorsedId { get; set; }  = string.Empty;
    public UserDto? Endorsed { get; set; }
    
    public string Content { get; set; } = string.Empty; 
    
    public IndicationStatusEnum Status { get; set; } 
    
    public DateTime? AcceptanceDate { get; set; } 
    
    public int? SkillRating { get; set; }
}