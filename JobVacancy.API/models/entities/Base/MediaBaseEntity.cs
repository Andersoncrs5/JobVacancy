using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JobVacancy.API.models.entities.Base;

public class MediaBaseEntity: BaseEntity
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
    [Column(TypeName = "SMALLINT")]
    public int? Order { get; set; }
}