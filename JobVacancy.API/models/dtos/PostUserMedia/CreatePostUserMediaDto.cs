using System.ComponentModel.DataAnnotations;
using JobVacancy.API.Utils.Annotations.Validations.Global;

namespace JobVacancy.API.models.dtos.PostUserMedia;

public class CreatePostUserMediaDto
{
    [Required]
    [MaxFileSize(30)]
    [AllowedExtensions([
        ".png", 
        ".jpg", 
        ".jpeg", 
        ".webp", 
        ".gif", 
        ".svg", 
        ".heic",
        ".tiff"
    ])]
    public required IFormFile File { get; set; }
    
    [Range(0, 15)]
    public int? Order { get; set; }
    
}