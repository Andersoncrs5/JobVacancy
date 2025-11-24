using JobVacancy.API.models.dtos.Users;

namespace JobVacancy.API.models.dtos.Resume;

public class ResumeDto: BaseDto
{
    public string Name { get; set; }
    public string Url { get; set; }
    public int? Version { get; set; }
    public string BucketName { get; set; } 
    public string ObjectKey { get; set; }
    
    public string userId { get; set; }
    public UserDto? User { get; set; }
}