namespace JobVacancy.API.models.entities;

public class ResumeEntity: BaseEntity
{
    public string Name { get; set; }
    public string Url { get; set; }
    public int? Version { get; set; }
    public string BucketName { get; set; } 
    public string ObjectKey { get; set; }
    
    public string userId { get; set; }
    public UserEntity? User { get; set; }
}