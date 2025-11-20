using JobVacancy.API.models.entities;
using Microsoft.EntityFrameworkCore;

namespace JobVacancy.API.Utils.Filters.UserContentReaction;

public class UserContentReactionFilterQuery
{
    public static IQueryable<UserContentReactionEntity> ApplyFilter(IQueryable<UserContentReactionEntity> query, UserContentReactionFilterParams parameters)
    {
        if (parameters.LoadUser.HasValue)
            query = query.Include(x => x.User);
        
        if (parameters.LoadPostUser.HasValue)
            query = query.Include(x => x.PostUser);
        
        if (parameters.LoadPostEnterprise.HasValue)
            query = query.Include(x => x.PostEnterprise);
        
        if (parameters.LoadCommentUser.HasValue)
            query = query.Include(x => x.CommentUser);
        
        if (parameters.LoadCommentEnterprise.HasValue)
            query = query.Include(x => x.CommentEnterprise);
        
        return FilterBaseQuery.ApplyBaseFilters(query, parameters);
    }
}