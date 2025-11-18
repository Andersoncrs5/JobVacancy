namespace JobVacancy.API.Utils.Filters.ReviewUser;

public class ReviewUserFilterParams: FilterBaseParams
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    
    public bool? IsAnonymous { get; set; }
    public bool? Recommendation { get; set; }
    
    public int? RatingOverallMin { get; set; }
    public int? RatingOverallMax { get; set; }
    
    public int? RatingCultureMin { get; set; }
    public int? RatingCultureMax { get; set; }
    
    public int? RatingCompensationMin { get; set; }
    public int? RatingCompensationMax { get; set; }
    
    public int? RatingWorkLifeBalanceMin { get; set; }
    public int? RatingWorkLifeBalanceMax { get; set; }
    
    public int? RatingManagementMin { get; set; }
    public int? RatingManagementMax { get; set; }
    
    public string? ActorId { get; set; }
    public string? ActorName { get; set; }
    public string? ActorEmail { get; set; }
    
    public string? TargetId { get; set; }
    public string? TargetName { get; set; }
    public string? TargetEmail { get; set; }
    
    public bool? LoadTarget { get; set; }
    public bool? LoadActor { get; set; }
    
}