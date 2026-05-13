using Zuhid.Notification.Shared;

namespace Zuhid.Notification.VerifyEmail;

public class VerifyEmailMessage : IMessage
{
    public string Email { get; set; } = string.Empty;
    public string AppUrl { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
