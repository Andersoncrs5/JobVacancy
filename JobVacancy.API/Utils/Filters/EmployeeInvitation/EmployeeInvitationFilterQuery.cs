using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.EmployeeInvitation;

public class EmployeeInvitationFilterQuery
{
    public static IQueryable<EmployeeInvitationEntity> ApplyFilter(IQueryable<EmployeeInvitationEntity> query,
        EmployeeInvitationFilterParam filter)
    {
        query = query.Include(x => x.User);
        query = query.Include(x => x.Enterprise);
        query = query.Include(x => x.Position);

        if (!string.IsNullOrEmpty(filter.UserId))
        {
            query = query.Where(x => x.UserId == filter.UserId);
        }
        
        if (!string.IsNullOrEmpty(filter.NameUser))
        {
            query = query.Where(x => x.User!.UserName!.Contains(filter.NameUser));
        }
        
        if (!string.IsNullOrEmpty(filter.EmailUser))
        {
            query = query.Where(x => x.User!.Email!.Contains(filter.EmailUser));
        }
        
        if (!string.IsNullOrEmpty(filter.EnterpriseId))
        {
            query = query.Where(x => x.EnterpriseId == filter.EnterpriseId);
        }

        if (!string.IsNullOrEmpty(filter.NameEnterprise))
        {
            query = query.Where(x => x.Enterprise!.Name.Contains(filter.NameEnterprise));
        }

        if (filter.TypeEnterprise.HasValue)
        {
            query = query.Where(x => x.Enterprise!.Type == filter.TypeEnterprise.Value);
        }
        
        if (!string.IsNullOrEmpty(filter.SalaryRange))
        {
            query = query.Where(x => x.SalaryRange.Contains(filter.SalaryRange));
        }

        if (filter.EmploymentType.HasValue)
        {
            query = query.Where(x => x.EmploymentType == filter.EmploymentType.Value);
        }

        if (filter.Status.HasValue)
        {
            query = query.Where(x => x.Status == filter.Status.Value);
        }

        if (filter.Currency.HasValue)
        {
            query = query.Where(x => x.Currency == filter.Currency.Value);
        }

        if (filter.ProposedStartDateMin.HasValue)
        {
            query = query.Where(x => x.ProposedStartDate >= filter.ProposedStartDateMin.Value);
        }
        if (filter.ProposedStartDateMax.HasValue)
        {
            query = query.Where(x => x.ProposedStartDate <= filter.ProposedStartDateMax.Value);
        }

        if (filter.ProposedEndDateMin.HasValue)
        {
            query = query.Where(x => x.ProposedEndDate.HasValue && x.ProposedEndDate.Value >= filter.ProposedEndDateMin.Value);
        }
        if (filter.ProposedEndDateMax.HasValue)
        {
            query = query.Where(x => x.ProposedEndDate.HasValue && x.ProposedEndDate.Value <= filter.ProposedEndDateMax.Value);
        }
        
        if (filter.ResponseDateMin.HasValue)
        {
            query = query.Where(x => x.ResponseDate.HasValue && x.ResponseDate.Value >= filter.ResponseDateMin.Value);
        }
        if (filter.ResponseDateMax.HasValue)
        {
            query = query.Where(x => x.ResponseDate.HasValue && x.ResponseDate.Value <= filter.ResponseDateMax.Value);
        }

        if (!string.IsNullOrEmpty(filter.InviteId))
        {
            query = query.Where(x => x.InviteSenderId == filter.InviteId);
        }

        if (!string.IsNullOrEmpty(filter.NameInvite) || !string.IsNullOrEmpty(filter.EmailInvite))
        {
            query = query.Include(x => x.InviteSender);
        }

        if (!string.IsNullOrEmpty(filter.NameInvite))
        {
            query = query.Where(x => x.InviteSender!.UserName!.Contains(filter.NameInvite));
        }
        
        if (!string.IsNullOrEmpty(filter.EmailInvite))
        {
            query = query.Where(x => x.InviteSender!.Email!.Contains(filter.EmailInvite));
        }
        
        if (!string.IsNullOrEmpty(filter.PositionId))
        {
            query = query.Where(x => x.PositionId == filter.PositionId);
        }
        
        if (!string.IsNullOrEmpty(filter.NamePosition))
        {
            query = query.Where(x => x.Position!.Name.Contains(filter.NamePosition));
        }
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}