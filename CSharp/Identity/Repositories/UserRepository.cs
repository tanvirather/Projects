using Microsoft.AspNetCore.Identity;
using Zuhid.Identity.Entities;

namespace Zuhid.Identity.Repositories;

public class UserRepository(UserManager<User> userManager, SignInManager<User> signInManager)
{
    public virtual async Task<(SignInResult Result, User? User)> Login(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return (SignInResult.Failed, null);
        }

        var result = await signInManager.PasswordSignInAsync(user, password, false, false);
        return (result, result.Succeeded || result.RequiresTwoFactor ? user : null);
    }

    public async Task<string?> GetAuthenticatorKeyAsync(User user)
    {
        return await userManager.GetAuthenticatorKeyAsync(user);
    }

    public async Task<IdentityResult> ResetAuthenticatorKeyAsync(User user)
    {
        return await userManager.ResetAuthenticatorKeyAsync(user);
    }

    public async Task<bool> VerifyTwoFactorTokenAsync(User user, string code)
    {
        return await userManager.VerifyTwoFactorTokenAsync(user, userManager.Options.Tokens.AuthenticatorTokenProvider, code);
    }

    public async Task<IdentityResult> SetTwoFactorEnabledAsync(User user, bool enabled)
    {
        return await userManager.SetTwoFactorEnabledAsync(user, enabled);
    }

    public virtual async Task<(SignInResult Result, User? User)> TwoFactorAuthenticatorSignInAsync(string code, bool isPersistent, bool rememberClient)
    {
        var result = await signInManager.TwoFactorAuthenticatorSignInAsync(code, isPersistent, rememberClient);
        if (result.Succeeded)
        {
            return (result, await signInManager.GetTwoFactorAuthenticationUserAsync());
        }
        return (result, null);
    }

    public async Task<(User? User, List<KeyValuePair<string, string>>? errors)> Add(User user, string password)
    {
        var result = await userManager.CreateAsync(user, password);
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

    internal async Task<string> GenerateChangePhoneNumberTokenAsync(User user, string phoneNumber)
    {
        var token = await userManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
        return token;
    }

    public async Task<List<KeyValuePair<string, string>>?> ConfirmEmailAsync(string email, string token)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            return [new KeyValuePair<string, string>("UserNotFound", "User not found")];
        }

        var result = await userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded
            ? null
            : [.. result.Errors.Select(e => new KeyValuePair<string, string>(e.Code, e.Description))];
    }

    public async Task<(User?, string)> GenerateChangePhoneNumberTokenAsync(Guid id)
    {
        var user = await userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            return (null,string.Empty);
        }
        var token = await userManager.GenerateChangePhoneNumberTokenAsync(user, user.PhoneNumber!);
        return (user, token);
    }

    public async Task<List<KeyValuePair<string, string>>?> Update(User user)
    {
        var foundUser = await userManager.FindByIdAsync(user.Id.ToString());
        if (foundUser == null)
        {
            return [new KeyValuePair<string, string>("UserNotFound", "User not found")];
        }

        foundUser.FirstName = user.FirstName;
        foundUser.LastName = user.LastName;
        foundUser.PhoneNumber = user.PhoneNumber;

        var result = await userManager.UpdateAsync(foundUser);
        return result.Succeeded
            ? null
            : [.. result.Errors.Select(e => new KeyValuePair<string, string>(e.Code, e.Description))];
    }

    public virtual async Task<User?> GetById(Guid id)
    {
        return await userManager.FindByIdAsync(id.ToString());
    }

    public async Task<List<KeyValuePair<string, string>>?> ConfirmPhoneAsync(string userId, string token)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return [new KeyValuePair<string, string>("UserNotFound", "User not found")];
        }

        var result = await userManager.ChangePhoneNumberAsync(user, user.PhoneNumber!, token);
        return result.Succeeded
            ? null
            : [.. result.Errors.Select(e => new KeyValuePair<string, string>(e.Code, e.Description))];
    }
}

