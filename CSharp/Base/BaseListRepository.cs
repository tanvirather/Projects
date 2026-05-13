using Microsoft.EntityFrameworkCore;

namespace Zuhid.Base;

public class BaseListRepository<TContext>(TContext context) where TContext : DbContext
{
    public async Task<List<BaseListModel<TId>>> Get<TEntity, TId>() where TEntity : BaseListEntity<TId> where TId : struct, Enum => await context.Set<TEntity>()
        .Where(n => n.IsActive)
        .OrderBy(n => n.SortOrder ?? int.MinValue)
        .ThenBy(n => n.Text)
        .Select(entity => new BaseListModel<TId>
        {
            Id = entity.Id,
            Text = entity.Text
        })
        .ToListAsync();
}

