using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.FollowerUserRelationshipEnterprise;

public class FollowerUserRelationshipEnterpriseFilterQuery
{
    public static IQueryable<FollowerUserRelationshipEnterpriseEntity> ApplyFilter(
        IQueryable<FollowerUserRelationshipEnterpriseEntity> query,
        FollowerUserRelationshipEnterpriseFilterParam param)
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
        
        if (param.WishReceiveNotifyByNewComment.HasValue)
            query = query.Where(x => x.WishReceiveNotifyByNewComment == param.WishReceiveNotifyByNewComment.Value);
        
        if (param.WishReceiveNotifyByNewInteraction.HasValue)
            query = query.Where(x => x.WishReceiveNotifyByNewInteraction == param.WishReceiveNotifyByNewInteraction.Value);
        
        if (param.WishReceiveNotifyByNewVacancy.HasValue)
            query = query.Where(x => x.WishReceiveNotifyByNewVacancy == param.WishReceiveNotifyByNewVacancy.Value);
        
        return FilterBaseQuery.ApplyBaseFilters(query, param);
    }
}