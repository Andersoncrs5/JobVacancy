using JobVacancy.API.models.entities.Enums;

namespace JobVacancy.API.models.entities;

public class EmployeeEnterpriseEntity: BaseEntity
{
    public string UserId {get; set;}
    public UserEntity? User {get; set;}
    
    public string EnterpriseId {get; set;}
    public EnterpriseEntity? Enterprise {get; set;}
    
    public string Position {get; set;} = string.Empty;
    public string SalaryRange {get; set;} = string.Empty;
    
    public EmploymentTypeEnum EmploymentType {get; set;}
    public EmploymentStatusEnum EmploymentStatus {get; set;}
    
    public DateTime StartDate {get; set;}
    public DateTime EndDate {get; set;}
}