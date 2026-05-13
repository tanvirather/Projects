using Microsoft.AspNetCore.Identity;
using Zuhid.Base;
using Zuhid.Identity.Entities;

namespace Zuhid.Identity;

partial class Program
{
    static void Main(string[] args)
    {
        var (builder, appSetting) = WebApplicationExtension.AddServices<AppSetting>(args, ["Repository", "Mapper", "Validator", "Client"]);

        builder.AddPostgres<IdentityContext>(appSetting.ConnectionStrings.Identity);
        builder.Services
            .AddIdentity<User, Role>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        var app = builder.BuildServices(appSetting);
        app.Run();
    }
}
