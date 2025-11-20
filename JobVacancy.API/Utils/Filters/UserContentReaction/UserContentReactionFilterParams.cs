using JobVacancy.API.models.entities;

namespace JobVacancy.API.Utils.Filters.UserContentReaction;

public class UserContentReactionFilterParams: FilterBaseParams
{
    public bool? LoadUser { get; set; }
    public bool? LoadPostUser { get; set; }
    public bool? LoadPostEnterprise { get; set; }
    public bool? LoadCommentUser { get; set; }
    public bool? LoadCommentEnterprise { get; set; }
}