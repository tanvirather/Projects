using System.ComponentModel.DataAnnotations;

namespace Zuhid.Identity.Requests;

public record RegisterRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password,
    string FirstName,
    string LastName,
    string Phone
);
