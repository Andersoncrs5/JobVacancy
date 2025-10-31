namespace JobVacancy.API.Utils.Filters.PostUser;

public class PostUserFilterParam: BasePostParams
{
    public string? UserId { get; set; }
    public string? FullNameUser { get; set; }
    public string? UserName { get; set; }
    public string? EmailUser { get; set; }
}