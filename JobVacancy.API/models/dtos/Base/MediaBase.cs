using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.dtos.Base;

public class MediaBase: BaseDto
{
    public required string ObjectName { get; set; }
    public required string BucketName { get; set; }
    public string? VersionImage { get; set; }
    public long? FileSizeBytes { get; set; }
    public int? Order { get; set; }
}