namespace JobVacancy.API.models.dtos.ReviewEnterprise;

public class CreateReviewEnterpriseDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int RatingOverall { get; set; }
    
    public int? RatingCulture { get; set; }
    public int? RatingCompensation { get; set; }
    public int? RatingManagement { get; set; }
    public int? RatingWorkLifeBalance { get; set; }
    
    public bool IsAnonymous { get; set; }
}