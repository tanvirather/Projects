using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zuhid.Identity.Entities;
using Zuhid.Identity.NotificationClients;
using NotificationRequests = Zuhid.Identity.NotificationClients.Requests;
using Zuhid.Identity.Repositories;
using Zuhid.Identity.Requests;

using Zuhid.Identity.Validators;
using Zuhid.Identity.Providers;
using System.Text.Json;

namespace Zuhid.Identity.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(UserRepository userRepository, NotificationClient notificationClient, AppSetting appSetting, LoginValidator loginValidator, JwtProvider jwtProvider) : ControllerBase
{
    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task Register([FromBody] RegisterRequest request)
    {
        var (user, errors) = await userRepository.Add(new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.Phone
        }, request.Password);

        if (user != null)
        {
            var token = await userRepository.GenerateEmailConfirmationTokenAsync(user);
            await notificationClient.VerifyEmail(new NotificationRequests.VerifyEmailRequest(user.Email!, appSetting.AppUrl, token));
        }
        else
        {
            errors?.ForEach(e => ModelState.AddModelError(e.Key, e.Value));
        }
    }

    [HttpPost("VerifyEmail")]
    [AllowAnonymous]
    public async Task VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        var errors = await userRepository.ConfirmEmailAsync(request.Email, request.Token);
        errors?.ForEach(e => ModelState.AddModelError(e.Key, e.Value));
    }

    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var (result, user) = await userRepository.Login(request.Email, request.Password);

        if (result.RequiresTwoFactor)
        {
            return Ok(new { RequiresTwoFactor = true, UserId = user!.Id });
        }

        loginValidator.Validate(result, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = jwtProvider.GenerateToken(user!);
        return Ok(new { Token = token });
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get(Guid id)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine(JsonSerializer.Serialize(userId));

        var user = await userRepository.GetById(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost("ConfirmPhone")]
    public async Task ConfirmPhone([FromBody] ConfirmPhoneRequest request)
    {
        var errors = await userRepository.ConfirmPhoneAsync(request.Id.ToString(), request.Token);
        errors?.ForEach(e => ModelState.AddModelError(e.Key, e.Value));
    }

    [HttpPost("VerifyPhone/{id}")]
    public async Task VerifyPhone(Guid id)
    {
        var (user, token) = await userRepository.GenerateChangePhoneNumberTokenAsync(id);
        if (user == null)
        {
            return;
        }
        await notificationClient.VerifyPhone(new NotificationRequests.VerifyPhoneRequest(user.PhoneNumber, token));
    }

    [HttpPost("LoginTwoFactor")]
    public async Task<IActionResult> LoginTwoFactor([FromBody] TwoFactorLoginRequest request)
    {
        var user = await userRepository.GetById(request.UserId);
        if (user == null)
        {
            return NotFound();
        }

        var (result, signInUser) = await userRepository.TwoFactorAuthenticatorSignInAsync(request.Code, false, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("Code", "Invalid authenticator code.");
            return BadRequest(ModelState);
        }

        var token = jwtProvider.GenerateToken(signInUser!);
        return Ok(new { Token = token });
    }

    [HttpGet("EnableTwoFactor/{userId}")]
    public async Task<IActionResult> EnableTwoFactor(Guid userId)
    {
        // var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        Console.WriteLine(userId);
        var user = await userRepository.GetById(userId);
        if (user == null)
        {
            return NotFound();
        }

        var unformattedKey = await userRepository.GetAuthenticatorKeyAsync(user);
        if (string.IsNullOrEmpty(unformattedKey))
        {
            await userRepository.ResetAuthenticatorKeyAsync(user);
            unformattedKey = await userRepository.GetAuthenticatorKeyAsync(user);
        }

        var authenticatorUri = string.Format(
            "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6",
            "Zuhid",
            user.Email,
            unformattedKey);

        return Ok(new { SharedKey = unformattedKey, AuthenticatorUri = authenticatorUri });
    }

    [HttpPost("EnableTwoFactor")]
    public async Task<IActionResult> EnableTwoFactor([FromBody] EnableTwoFactorRequest request)
    {
        var user = await userRepository.GetById(request.UserId);
        if (user == null)
        {
            return NotFound();
        }

        var isValid = await userRepository.VerifyTwoFactorTokenAsync(user, request.Code);
        if (!isValid)
        {
            ModelState.AddModelError("Code", "Verification code is invalid.");
            return BadRequest(ModelState);
        }

        await userRepository.SetTwoFactorEnabledAsync(user, true);
        return Ok();
    }

    [HttpPut]
    public async Task Update([FromBody] UpdateAccountRequest request)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return;
        }

        var errors = await userRepository.Update(new User
        {
            Id = Guid.Parse(userId),
            FirstName = request.FirstName,
            LastName = request.LastName,
            PhoneNumber = request.Phone
        });

        if (errors == null)
        {
            var user = await userRepository.GetById(Guid.Parse(userId));
            if (user != null)
            {
                var token = await userRepository.GenerateChangePhoneNumberTokenAsync(user, request.Phone);
                await notificationClient.VerifyPhone(new NotificationRequests.VerifyPhoneRequest(request.Phone, token));
            }
        }

        errors?.ForEach(e => ModelState.AddModelError(e.Key, e.Value));
    }
}

