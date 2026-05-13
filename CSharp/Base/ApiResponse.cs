using System.Net;
namespace Zuhid.Base;

public class ApiResponse<TModel> where TModel : class
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Content { get; set; }
    public TModel? Model { get; set; }
}
