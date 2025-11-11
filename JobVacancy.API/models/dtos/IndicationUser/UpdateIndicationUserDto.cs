using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.IndicationUser;

public class UpdateIndicationUserDto
{
    public string? Content { get; set; }
    public IndicationStatusEnum? Status { get; set; }
    [Range(0, 10)]
    public int? SkillRating { get; set; }
}