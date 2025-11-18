using System.ComponentModel.DataAnnotations;
using JobVacancy.API.Utils.Annotations.Validations;

namespace JobVacancy.API.models.dtos.ReviewUser;

public class CreateReviewUserDto
{
    public string Title { get; set; } = string.Empty;
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

    public bool Recommendation { get; set; }
    public bool IsAnonymous { get; set; }
    
    [ExistsUserById]
    public required string TargetUserId { get; set; }
}