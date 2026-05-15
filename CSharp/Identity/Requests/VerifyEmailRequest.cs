using System.ComponentModel.DataAnnotations;

namespace Zuhid.Identity.Requests;

public record VerifyEmailRequest(
    [Required] string Email,
    [Required] string Token
);

