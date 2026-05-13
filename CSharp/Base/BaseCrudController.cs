using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Zuhid.Base;

[ApiController]
[Route("[controller]")]
public class BaseCrudController<TContext, TRepository, TEntity, TModel>(TRepository repository) : ControllerBase
    where TContext : DbContext
    where TRepository : BaseRepository<TContext, TEntity, TModel>
    where TEntity : class, IEntity
    where TModel : TEntity
{

    [HttpGet()]
    public virtual async Task<List<TModel>> Get(Guid id = default)
    {
        return await repository.Get(id).ConfigureAwait(false);
    }

    [HttpPost]
    public virtual async Task<UpdatedModel> Add([FromBody] TModel model)
    {
        return await repository.Add(model).ConfigureAwait(false);
    }

    [HttpPut()]
    public virtual async Task<UpdatedModel> Update([FromBody] TModel model)
    {
        return await repository.Update(model).ConfigureAwait(false);
    }

    [HttpDelete()]
    public virtual async Task Delete(Guid id)
    {
        await repository.Delete(id).ConfigureAwait(false);
    }
}
