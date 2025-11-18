using System.ComponentModel.DataAnnotations;

namespace JobVacancy.API.models.dtos.ReviewUser;

public class UpdateReviewUserDto
{
    [MaxLength(299)]
    public string? Title { get; set; }
    
    [MaxLength(800)]
    public string? Content { get; set; }
    [Range(0 , 5)]
    public int? RatingOverall { get; set; }
    [Range(0 , 5)]
    public int? RatingCulture { get; set; }
    [Range(0 , 5)]
    public int? RatingCompensation { get; set; }
    [Range(0 , 5)]
    public int? RatingManagement { get; set; }
    [Range(0 , 5)]
    public int? RatingWorkLifeBalance { get; set; }
    
    public bool? IsAnonymous { get; set; }
}