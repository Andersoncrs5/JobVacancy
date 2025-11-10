namespace JobVacancy.API.models.dtos.ReviewEnterprise;

public class UpdateReviewEnterpriseDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int? RatingOverall { get; set; }
    
    public int? RatingCulture { get; set; }
    public int? RatingCompensation { get; set; }
    public int? RatingManagement { get; set; }
    public int? RatingWorkLifeBalance { get; set; }
    
    public bool? IsAnonymous { get; set; }
}