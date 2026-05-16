namespace Zuhid.Identity.Requests;

public class TwoFactorLoginRequest
{
    public Guid UserId { get; set; }
    public string Code { get; set; } = string.Empty;
}
