using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.FollowerRelationshipUser;

public class FollowerRelationshipUserFilterQuery
{
    public static IQueryable<FollowerRelationshipUserEntity> ApplyFilter(IQueryable<FollowerRelationshipUserEntity> query, FollowerRelationshipUserFilterParams filter)
    {
        if (
            filter.LoadFollower.HasValue || 
            !(string.IsNullOrWhiteSpace(filter.UserNameFollower)) || 
            !(string.IsNullOrWhiteSpace(filter.EmailFollower)) || 
            !(string.IsNullOrWhiteSpace(filter.FullNameFollower)) 
            )
            query = query.Include(x => x.Follower);
        
        if (
            filter.LoadFollowed.HasValue || 
            !(string.IsNullOrWhiteSpace(filter.UserNameFollowed)) || 
            !(string.IsNullOrWhiteSpace(filter.EmailFollowed)) || 
            !(string.IsNullOrWhiteSpace(filter.FullNameFollowed)) 
            )
            query = query.Include(x => x.Followed);
        
        // FOLLOWER
        if (!string.IsNullOrWhiteSpace(filter.FollowerId))
            query = query.Where(x => x.FollowerId == filter.FollowerId);
        
        if  (!string.IsNullOrWhiteSpace(filter.UserNameFollower))
            query = query.Where(x => EF.Functions.Like(x.Follower!.UserName, $"%{filter.UserNameFollower}%"));
        
        if  (!string.IsNullOrWhiteSpace(filter.EmailFollower))
            query = query.Where(x => EF.Functions.Like(x.Follower!.Email, $"%{filter.EmailFollower}%"));
        
        if  (!string.IsNullOrWhiteSpace(filter.FullNameFollower))
            query = query.Where(x => EF.Functions.Like(x.Follower!.FullName, $"%{filter.FullNameFollower}%"));
        
        // FOLLOWED
        if (!string.IsNullOrWhiteSpace(filter.FollowedId))
            query = query.Where(x => x.FollowedId == filter.FollowedId);
        
        if  (!string.IsNullOrWhiteSpace(filter.UserNameFollowed))
            query = query.Where(x => EF.Functions.Like(x.Followed!.UserName, $"%{filter.UserNameFollower}%"));
        
        if  (!string.IsNullOrWhiteSpace(filter.EmailFollowed))
            query = query.Where(x => EF.Functions.Like(x.Followed!.Email, $"%{filter.EmailFollower}%"));
        
        if  (!string.IsNullOrWhiteSpace(filter.FullNameFollowed))
            query = query.Where(x => EF.Functions.Like(x.Followed!.FullName, $"%{filter.FullNameFollower}%"));
        
        // 
        
        if (filter.WishReceiveNotifyByNewPost.HasValue)
            query = query.Where(x => x.WishReceiveNotifyByNewPost == filter.WishReceiveNotifyByNewPost.Value);
        
        if (filter.WishReceiveNotifyByNewComment.HasValue)
            query = query.Where(x => x.WishReceiveNotifyByNewComment == filter.WishReceiveNotifyByNewComment.Value);
        
        if (filter.WishReceiveNotifyByNewInteraction.HasValue)
            query = query.Where(x => x.WishReceiveNotifyByNewInteraction == filter.WishReceiveNotifyByNewInteraction.Value);
        
        return FilterBaseQuery.ApplyBaseFilters(query, filter);
    }
}