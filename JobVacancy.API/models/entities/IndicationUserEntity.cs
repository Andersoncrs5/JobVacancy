using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class IndicationUserEntity: BaseEntity
{
    // Indicador
    public string EndorserId { get; set; }
    public UserEntity? Endorser { get; set; }

    // Indicado
    public string EndorsedId { get; set; } 
    public UserEntity? Endorsed { get; set; }
    
    public string Content { get; set; } = string.Empty; 
    
    public IndicationStatusEnum Status { get; set; } 
    
    public DateTime? AcceptanceDate { get; set; } 
    
    public int? SkillRating { get; set; }
}