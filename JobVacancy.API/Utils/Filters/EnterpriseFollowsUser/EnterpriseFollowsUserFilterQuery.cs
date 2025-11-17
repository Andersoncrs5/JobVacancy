using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.EnterpriseFollowsUser;

public class EnterpriseFollowsUserFilterQuery
{
    public static IQueryable<EnterpriseFollowsUserEntity> ApplyFilter(
        IQueryable<EnterpriseFollowsUserEntity> query,
        EnterpriseFollowsUserFilterParam param)
    {

        if (
            param.LoadUser.HasValue ||
            !string.IsNullOrWhiteSpace(param.UserName) ||
            !string.IsNullOrWhiteSpace(param.Email) ||
            !string.IsNullOrWhiteSpace(param.FullName)
        )
            query = query.Include(x=>x.User);
        
        if (
            param.LoadEnterprise.HasValue ||
            !string.IsNullOrWhiteSpace(param.Name) || 
            param.Type.HasValue
            )
            query = query.Include(x=>x.Enterprise);
        
        // USER
        if (!string.IsNullOrWhiteSpace(param.UserId))
            query = query.Where(x => x.UserId == param.UserId);
        
        if (!string.IsNullOrWhiteSpace(param.UserName))
            query = query.Where(x => EF.Functions.Like(x.User!.UserName, $"%{param.UserName}%"));
        
        if (!string.IsNullOrWhiteSpace(param.Email))
            query = query.Where(x => EF.Functions.Like(x.User!.Email, $"%{param.Email}%"));
        
        if (!string.IsNullOrWhiteSpace(param.FullName))
            query = query.Where(x => EF.Functions.Like(x.User!.FullName, $"%{param.FullName}%"));
        
        // ENTERPRISE
        if (!string.IsNullOrWhiteSpace(param.EnterpriseId))
            query = query.Where(x => x.EnterpriseId == param.EnterpriseId);

        if (!string.IsNullOrWhiteSpace(param.Name))
            query = query.Where(x => EF.Functions.Like(x.Enterprise!.Name, $"%{param.Name}%"));
        
        if (param.Type.HasValue)
            query = query.Where(x => x.Enterprise!.Type == param.Type);
        
        //
        if (param.WishReceiveNotifyByNewPost.HasValue)
            query = query.Where(x => x.WishReceiveNotifyByNewPost == param.WishReceiveNotifyByNewPost.Value);
        
        if (param.WishReceiveNotifyByNewEndorsement.HasValue)
            query = query.Where(x => x.WishReceiveNotifyByNewEndorsement == param.WishReceiveNotifyByNewEndorsement.Value);
        
        if (param.WishReceiveNotifyByNewInteraction.HasValue)
            query = query.Where(x => x.WishReceiveNotifyByNewInteraction == param.WishReceiveNotifyByNewInteraction.Value);
        
        if (param.WishReceiveNotifyByProfileUpdate.HasValue)
            query = query.Where(x => x.WishReceiveNotifyByProfileUpdate == param.WishReceiveNotifyByProfileUpdate.Value);
        
        return FilterBaseQuery.ApplyBaseFilters(query, param);
    }
}