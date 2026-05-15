using System.ComponentModel.DataAnnotations;

namespace Zuhid.Identity.Requests;

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password
);
