using System.ComponentModel.DataAnnotations;
using JobVacancy.API.Utils.Annotations.Validations;

namespace JobVacancy.API.models.dtos.Position;

public class CreatePositionDto
{
    [Required]
    [MaxLength(200)]
    [UniquePositionName]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(400)]
    public string? Describe { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
}