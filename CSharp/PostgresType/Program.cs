using Zuhid.Base;

namespace Zuhid.PostgresType;

partial class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var appSetting = new AppSetting(builder.Configuration);
        builder.AddServices(appSetting, ["Repository", "Mapper", "Validator"]);
        var app = builder.BuildServices(appSetting);
        app.Run();
    }
}
