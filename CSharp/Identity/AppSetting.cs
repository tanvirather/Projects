using Zuhid.Base;

namespace Zuhid.Identity;

public class AppSetting : BaseSetting
{
    public string AppUrl { get; set; } = default!;
    public ConnectionString ConnectionStrings { get; set; } = default!;
    public NotificationOptions Notification { get; set; } = default!;

    public class ConnectionString
    {
        public string Identity { get; set; } = default!;
        public string Log { get; set; } = default!;
    }

    public class NotificationOptions : IApiClientOptions
    {
        public string BaseUrl { get; set; } = default!;

        public string Authorization { get; set; } = default!;
    }

    public override void Bind(IConfiguration configuration)
    {
        configuration.Bind(this);
        ConnectionStrings = new ConnectionString
        {
            Identity = ReplaceCredential(configuration, "Identity"),
            Log = ReplaceCredential(configuration, "Log"),
        };
    }

    private static string ReplaceCredential(IConfiguration configuration, string connString)
    {
        return configuration.GetConnectionString(connString)!
          .Replace("[postgres_credential]", configuration.GetValue<string>("postgres_credential"), StringComparison.Ordinal);
    }
}
