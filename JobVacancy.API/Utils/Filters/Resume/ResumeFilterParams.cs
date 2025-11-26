namespace JobVacancy.API.Utils.Filters.Resume;

public class ResumeFilterParams: FilterBaseParams
{
    public string? Name { get; set; }
    public int? VersionMin { get; set; }
    public int? VersionMax{ get; set; }
    public string? BucketName { get; set; } 
    public string? ObjectKey { get; set; }
    
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    
    public bool? LoadUser { get; set; }
}