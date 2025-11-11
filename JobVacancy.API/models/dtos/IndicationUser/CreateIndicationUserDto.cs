using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.IndicationUser;

public class CreateIndicationUserDto
{
    [Required]
    public string EndorsedId { get; set; }  = string.Empty;
    [Required]
    public string Content { get; set; } = string.Empty;
    [Required]
    public IndicationStatusEnum Status { get; set; }
    [Range(0, 10)]
    public int? SkillRating { get; set; }
}