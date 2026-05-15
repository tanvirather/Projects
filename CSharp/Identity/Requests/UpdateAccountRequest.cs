namespace Zuhid.Identity.Requests;

public record UpdateAccountRequest(
    string FirstName,
    string LastName,
    string Phone
);
