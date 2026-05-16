namespace Zuhid.Identity.NotificationClients.Requests;

public record VerifyPhoneRequest
(
    string Phone,
    string Token
);
