using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.dtos.Resume;

public class CreateResumeDto
{
    public required string Name { get; set; }
    public int? Version { get; set; }
    [Required]
    public required IFormFile File { get; set; }
}