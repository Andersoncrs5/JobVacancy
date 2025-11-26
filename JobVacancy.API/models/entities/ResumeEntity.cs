namespace JobVacancy.API.models.entities;

public class ResumeEntity: BaseEntity
{
    public required string Name { get; set; }
    public string? Url { get; set; }
    public int? Version { get; set; }
    public required string BucketName { get; set; } 
    public required string ObjectKey { get; set; }
    
    public string userId { get; set; }
    public UserEntity? User { get; set; }
}