using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.dtos.Base;

public class MediaBase
{
    [Required]
    [MaxLength(450)]
    public required string ObjectName { get; set; }
    
    [Required]
    [MaxLength(100)]
    public required string BucketName { get; set; }
    
    [MaxLength(3)]
    public string? VersionImage { get; set; }
    
    public long? FileSizeBytes { get; set; }
    
    [Range(0,15)]
    public int? Order { get; set; }
}