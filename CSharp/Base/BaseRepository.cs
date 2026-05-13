using Microsoft.EntityFrameworkCore;

namespace Zuhid.Base;

public abstract class BaseRepository<TContext, TEntity, TModel>(TContext context) where TContext : DbContext
  where TEntity : class, IEntity
  where TModel : TEntity
{
    protected abstract IQueryable<TModel> Query { get; }

    public async Task<List<TModel>> Get(Guid id = default) => id == Guid.Empty
        ? await Query.Skip(0).Take(50).ToListAsync()
        : await Query.Where(n => n.Id.Equals(id)).ToListAsync();

    public async Task<UpdatedModel> Add(TEntity entity)
    {
        try
        {
            entity.Updated = DateTime.UtcNow;
            context.Set<TEntity>().Add(entity);
            await context.SaveChangesAsync();
            return new UpdatedModel { Updated = entity.Updated };
        }
        catch (Exception ex)
        {
            ex.Data.Add("entity", entity);
            throw;
        }
    }

    public async Task<UpdatedModel> Update(TEntity entity)
    {
        try
        {
            entity.Updated = DateTime.UtcNow;
            context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
            return new UpdatedModel { Updated = entity.Updated };
        }
        catch (Exception ex)
        {
            ex.Data.Add("entity", entity);
            throw;
        }
    }

    public async Task Delete(Guid id)
    {
        await context.Set<TEntity>().Where(e => e.Id.Equals(id)).ExecuteDeleteAsync();
    }
}
