using JobVacancy.API.models.dtos.Enterprise;

namespace JobVacancy.API.models.dtos.PostEnterprise;

public class PostEnterpriseDto: BasePost
{
    public EnterpriseDto? Enterprise { get; set; }
}