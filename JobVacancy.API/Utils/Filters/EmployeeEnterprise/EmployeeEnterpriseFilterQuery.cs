using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.EmployeeEnterprise;

public class EmployeeEnterpriseFilterQuery
{
    public static IQueryable<EmployeeEnterpriseEntity> ApplyFilter(IQueryable<EmployeeEnterpriseEntity> query,
        EmployeeEnterpriseFilterParam filter)
    {
        query = query.Include(x => x.User);
        query = query.Include(x => x.Position);

        if (
            !string.IsNullOrEmpty(filter.EnterpriseId) ||
            !string.IsNullOrEmpty(filter.NameEnterprise) ||
            filter.Type.HasValue
        )
        {
            query = query.Include(x => x.Enterprise);
        }
        
        if (
            !string.IsNullOrEmpty(filter.InviteId) ||
            !string.IsNullOrEmpty(filter.InviteName) ||
            !string.IsNullOrEmpty(filter.InviteEmail)
        )
        {
            query = query.Include(x => x.InviteSender);
        }
        
        if (!string.IsNullOrEmpty(filter.UserId))
        {
            query = query.Where(x => x.UserId == filter.UserId);
        }
        
        if (!string.IsNullOrEmpty(filter.Username))
        {
            query = query.Where(x => x.User!.UserName!.Contains(filter.Username));
        }
        
        if (!string.IsNullOrEmpty(filter.UserEmail))
        {
            query = query.Where(x => x.User!.Email!.Contains(filter.UserEmail));
        }
        
        if (!string.IsNullOrEmpty(filter.EnterpriseId))
        {
            query = query.Where(x => x.EnterpriseId == filter.EnterpriseId);
        }

        if (!string.IsNullOrEmpty(filter.NameEnterprise))
        {
            query = query.Where(x => x.Enterprise!.Name.Contains(filter.NameEnterprise));
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(x => x.Enterprise!.Type == filter.Type.Value);
        }

        if (!string.IsNullOrEmpty(filter.SalaryRange))
        {
            query = query.Where(x => x.SalaryRange.Equals(filter.SalaryRange));
        }

        if (filter.SalaryValueMin.HasValue)
        {
            query = query.Where(x => x.SalaryValue >= filter.SalaryValueMin);
        }
        
        if (filter.SalaryValueMax.HasValue)
        {
            query = query.Where(x => x.SalaryValue <= filter.SalaryValueMax);
        }

        if (filter.PaymentFrequency.HasValue)
        {
            query = query.Where(x => x.PaymentFrequency == filter.PaymentFrequency.Value);
        }

        if (filter.ContractLegalType.HasValue)
        {
            query = query.Where(x => x.ContractLegalType == filter.ContractLegalType.Value);
        }

        if (filter.EmploymentType.HasValue)
        {
            query = query.Where(x => x.EmploymentType == filter.EmploymentType.Value);
        }
        
        if (filter.EmploymentStatus.HasValue)
        {
            query = query.Where(x => x.EmploymentStatus == filter.EmploymentStatus.Value);
        }
        
        if (filter.Currency.HasValue)
        {
            query = query.Where(x => x.Currency == filter.Currency.Value);
        }
        
        if (filter.StartDateMin.HasValue)
        {
            query = query.Where(x => x.StartDate >= filter.StartDateMin.Value);
            
        }
        
        if (filter.StartDateMax.HasValue)
        {
            query = query.Where(x => x.StartDate <= filter.StartDateMax.Value);
        }

        if (filter.EndDateMin.HasValue)
        {
            query = query.Where(x => x.EndDate.HasValue && x.EndDate.Value >= filter.EndDateMin.Value);
        }
        
        if (filter.EndDateMax.HasValue)
        {
            query = query.Where(x => x.EndDate.HasValue && x.EndDate.Value <= filter.EndDateMax.Value);
        }

        if (!string.IsNullOrEmpty(filter.PositionId))
        {
            query = query.Where(x => x.PositionId.Equals(filter.PositionId));
        }

        if (!string.IsNullOrEmpty(filter.NamePosition))
        {
            query = query.Where(x => x.Position!.Name.Contains(filter.NamePosition));
        }
        
        if (!string.IsNullOrEmpty(filter.InviteId))
        {
            query = query.Where(x => x.InviteSenderId == filter.InviteId);
        }
        
        if (!string.IsNullOrEmpty(filter.InviteName))
        {
            query = query.Where(x => x.InviteSender!.UserName!.Contains(filter.InviteName));
        }
        
        if (!string.IsNullOrEmpty(filter.InviteEmail))
        {
            query = query.Where(x => x.InviteSender!.Email!.Contains(filter.InviteEmail));
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}