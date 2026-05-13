using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zuhid.Identity.Entities;
using Zuhid.Identity.NotificationClients;
using Zuhid.Identity.NotificationClients.Requests;
using Zuhid.Identity.Repositories;
using Zuhid.Identity.Requests;

namespace Zuhid.Identity.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController(UserRepository userRepository, NotificationClient notificationClient, AppSetting appSetting) : ControllerBase
{
    [HttpPost("Register")]
    [AllowAnonymous]
    public async Task Register([FromBody] RegisterRequest request)
    {
        var (user, errors) = await userRepository.Add(new User
        {
            UserName = request.Email,
            Email = request.Email
        });
        if (user != null)
        {
            var token = await userRepository.GenerateEmailConfirmationTokenAsync(user);
            await notificationClient.VerifyEmail(new VerifyEmailRequest(user.Email!, appSetting.AppUrl, token));
        }
        else
        {
            errors?.ForEach(e => ModelState.AddModelError(e.Key, e.Value));
        }
    }

    [HttpPost("AppSetting")]
    [AllowAnonymous]
    public AppSetting AppSetting()
    {
        return appSetting;
    }
}

