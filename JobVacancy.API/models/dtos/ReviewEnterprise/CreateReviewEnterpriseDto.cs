using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.dtos.ReviewEnterprise;

public class CreateReviewEnterpriseDto
{
    [Required]
    [MaxLength(299)]
    public string Title { get; set; } = string.Empty;
    [Required]
    [MaxLength(800)]
    public string Content { get; set; } = string.Empty;
    [Required]
    [Range(0 , 5)]
    public int RatingOverall { get; set; }
    
    [Range(0 , 5)]   
    public int? RatingCulture { get; set; }
    [Range(0 , 5)]
    public int? RatingCompensation { get; set; }
    [Range(0 , 5)]
    public int? RatingManagement { get; set; }
    [Range(0 , 5)]
    public int? RatingWorkLifeBalance { get; set; }
    
    [Required]
    public bool IsAnonymous { get; set; }
}