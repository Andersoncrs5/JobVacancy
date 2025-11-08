using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.dtos.Position;

public class UpdatePositionDto
{
    public string? Name { get; set; }
    public string? Describe { get; set; }
    public bool? IsActive { get; set; }
}