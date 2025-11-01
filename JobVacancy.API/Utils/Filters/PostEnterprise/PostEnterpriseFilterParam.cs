using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.Utils.Filters.PostUser;

public class PostEnterpriseFilterParam: BasePostParams
{
    public string? NameEnterprise { get; set; }
    public EnterpriseTypeEnum? TypeEnterprise { get; set; }
}