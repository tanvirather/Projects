using Zuhid.Base;

namespace Zuhid.PostgresType;

public class AppSetting : BaseSetting
{
    public string AppUrl { get; set; } = default!;
    public ConnectionString ConnectionStrings { get; set; } = default!;
    public class ConnectionString
    {
        public string PostgresType { get; set; } = default!;
        public string Log { get; set; } = default!;
    }

    public AppSetting(IConfiguration configuration) : base(configuration)
    {
        ConnectionStrings = new ConnectionString
        {
            PostgresType = ReplaceCredential(configuration, "PostgresType"),
            Log = ReplaceCredential(configuration, "Log"),
        };
    }

    private static string ReplaceCredential(IConfiguration configuration, string connString)
    {
        return configuration.GetConnectionString(connString)!
          .Replace("[postgres_credential]", configuration.GetValue<string>("postgres_credential"), StringComparison.Ordinal);
    }
}
