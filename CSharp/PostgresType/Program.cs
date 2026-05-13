using Zuhid.Base;

namespace Zuhid.PostgresType;

partial class Program
{
    static void Main(string[] args)
    {
        var (builder, appSetting) = WebApplicationExtension.AddServices<AppSetting>(args, ["Repository", "Mapper", "Validator"]);
        var app = builder.BuildServices(appSetting);
        app.Run();
    }
}
