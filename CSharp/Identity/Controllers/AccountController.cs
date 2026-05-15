using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zuhid.Identity.Entities;
using Zuhid.Identity.NotificationClients;
using NotificationRequests = Zuhid.Identity.NotificationClients.Requests;
using Zuhid.Identity.Repositories;
using Zuhid.Identity.Requests;

using Zuhid.Identity.Validators;
using Zuhid.Identity.Providers;

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
        loginValidator.Validate(result, ModelState);

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var token = jwtProvider.GenerateToken(user!);
        return Ok(new { Token = token });
    }

    [HttpPut]
    [AllowAnonymous]
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

        errors?.ForEach(e => ModelState.AddModelError(e.Key, e.Value));
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> Get(Guid id)
    {
        var user = await userRepository.GetById(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
}

