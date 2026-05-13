using System.ComponentModel.DataAnnotations.Schema;

namespace Zuhid.Base;

public class BaseListModel<TId> where TId : struct, Enum
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public TId Id { get; set; }
    public string Text { get; set; } = string.Empty;
}
