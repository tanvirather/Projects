namespace Zuhid.Identity.NotificationClients.Requests;

public record VerifyEmailRequest
(
    string Email,
    string AppUrl,
    string Token
);
