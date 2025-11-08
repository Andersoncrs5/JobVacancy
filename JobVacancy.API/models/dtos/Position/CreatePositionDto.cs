using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.dtos.Position;

public class CreatePositionDto
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(400)]
    public string? Describe { get; set; }
    
    [Required]
    public bool IsActive { get; set; }
}