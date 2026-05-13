using Zuhid.Base;

namespace Zuhid.Notification.Shared;

public class AppSetting : BaseSetting
{
    public SmtpSetting Smtp { get; init; } = default!;
    public class SmtpSetting
    {
        public string Host { get; init; } = default!;
        public int Port { get; init; }
        public string From { get; init; } = default!;
        public int RetryCount { get; init; }
        public TimeSpan RetryInterval { get; init; }
    }

    public override void Bind(IConfiguration configuration)
    {
        configuration.Bind(this);
    }
}
