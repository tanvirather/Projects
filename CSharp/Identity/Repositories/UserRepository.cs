using Microsoft.AspNetCore.Identity;
using Zuhid.Identity.Entities;

namespace Zuhid.Identity.Repositories;

public class UserRepository(UserManager<User> userManager)
{
    public async Task<(User? User, List<KeyValuePair<string, string>>? errors)> Add(User user)
    {
        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => new KeyValuePair<string, string>(e.Code, e.Description)).ToList();
            return (null, errors);
        }
        var foundUser = await userManager.FindByEmailAsync(user.Email!);
        return (foundUser, null);
    }

    internal async Task<string> GenerateEmailConfirmationTokenAsync(User user)
    {
        var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
        return token;
    }
}

