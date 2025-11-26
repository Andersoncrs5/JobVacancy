using System.ComponentModel.DataAnnotations;
using JobVacancy.API.Utils.Annotations.Validations.Global;

namespace JobVacancy.API.models.dtos.Resume;

public class CreateResumeDto
{
    [Required]
    public required string Name { get; set; }
    [Range(0, 100)]
    public int? Version { get; set; }
    [Required]
    [MaxFileSize(10)]
    [AllowedExtensions([".pdf", ".docx", ".doc", ".odt", ".rtf", ".txt", ".png", ".jpg", ".jpeg", ".htm", ".html"])]
    public required IFormFile File { get; set; }
}