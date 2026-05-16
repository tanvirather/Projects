using System.ComponentModel.DataAnnotations;

namespace Zuhid.Identity.Requests;

public record ConfirmPhoneRequest(
    [Required] Guid Id,
    [Required] string Token
);
