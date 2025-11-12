using System.ComponentModel.DataAnnotations;
using JobVacancy.API.Utils.Annotations.Validations;

namespace JobVacancy.API.models.dtos.Area;

public class CreateAreaDto
{
    [Required]
    [MaxLength(150)]
    [ExistsAreaByName]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Description { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
}