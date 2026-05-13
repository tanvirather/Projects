using Zuhid.Base;
using Zuhid.Identity.NotificationClients.Requests;

namespace Zuhid.Identity.NotificationClients;

public class NotificationClient(HttpClient httpClient, AppSetting appSetting, ILogger<NotificationClient> logger)
    : ApiClient(httpClient, appSetting.Notification, logger)
{
    public override Task<bool> Login()
    {
        return Task.FromResult(true);
    }

    public virtual async Task VerifyEmail(VerifyEmailRequest request) => await Post("Identity/VerifyEmail", request);
}
