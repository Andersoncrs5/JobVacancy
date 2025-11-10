using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Utils.Filters.ReviewEnterprise;

public class ReviewEnterpriseFilterParams: FilterBaseParams
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    
    public bool? IsAnonymous { get; set; }
    
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
    
    public string? PositionId { get; set; }
    public string? NamePosition { get; set; }
    
    public string? UserId { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    
    public string? EnterpriseId { get; set; }
    public string? EnterpriseName { get; set; }
    public EnterpriseTypeEnum? TypeEnterprise { get; set; }
}