namespace Zuhid.Base;

public class BaseListEntity<TId> where TId : struct, Enum
{
    public TId Id { get; set; }
    public Guid UpdatedById { get; set; }
    public DateTime Updated { get; set; }
    public bool IsActive { get; set; }
    public string Text { get; set; } = string.Empty;
    public int? SortOrder { get; set; }
}
