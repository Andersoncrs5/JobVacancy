using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Utils.Filters.IndicationUser;

public class IndicationUserFilterParam: FilterBaseParams
{
    public string? EndorserId { get; set; }
    public string? EndorserName { get; set; }
    public string? EndorserEmail { get; set; }
    
    public string? EndorsedId { get; set; }
    public string? EndorsedName { get; set; }
    public string? EndorsedEmail { get; set; }
    
    public string? Content { get; set; }
    public IndicationStatusEnum? Status { get; set; }
    public int? SkillRatingMin { get; set; }
    public int? SkillRatingMax { get; set; }
}