using Microsoft.AspNetCore.Identity;
using Zuhid.Base;
using Zuhid.Identity.Entities;

namespace Zuhid.Identity;

partial class Program
{
    static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services
            .AddIdentity<User, Role>()
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();

        var appSetting = new AppSetting(builder.Configuration);
        builder.AddServices(appSetting, ["Repository", "Mapper", "Validator", "Client", "Provider"]);
        builder.AddPostgres<IdentityContext>(appSetting.ConnectionStrings.Identity);
        var app = builder.BuildServices(appSetting);
        app.Run();
    }
}
