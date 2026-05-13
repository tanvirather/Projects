using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Zuhid.Base;

[ApiController]
[Route("[controller]")]

public class BaseListController<TContext, TEntity, TId>(BaseListRepository<TContext> repository) : ControllerBase
  where TContext : DbContext
  where TEntity : BaseListEntity<TId>
  where TId : struct, Enum
{
    [HttpGet()]
    public virtual async Task<List<BaseListModel<TId>>> Get()
    {
        return await repository.Get<TEntity, TId>();
    }
}
