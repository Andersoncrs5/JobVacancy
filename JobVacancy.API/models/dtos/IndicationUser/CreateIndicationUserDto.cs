using System.ComponentModel.DataAnnotations;
using JobVacancy.API.models.entities.Enums;
using JobVacancy.API.Utils.Annotations.Validations;

namespace JobVacancy.API.models.dtos.IndicationUser;

public class CreateIndicationUserDto
{
    [Required]
    [ExistsUserById]
    public string EndorsedId { get; set; }  = string.Empty;
    [Required]
    [MaxLength(500)]
    public string Content { get; set; } = string.Empty;
    [Range(0, 10)]
    public int? SkillRating { get; set; }
}