using JobVacancy.API.models.entities;
using JobVacancy.API.Utils.Page;

namespace JobVacancy.API.Utils.Filters.User;

public class UserFilterParams: FilterBaseParams
{
    public string? FullName { get; set; }
    public string? UserName { get; set; }
    public string? Email { get; set; }
    
}