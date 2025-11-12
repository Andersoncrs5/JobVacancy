using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.dtos.IndicationUser;

public class UpdateIndicationUserEndorserDto
{
    public string? Content { get; set; }
    [Range(0, 10)]
    public int? SkillRating { get; set; }
}