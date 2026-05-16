using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zuhid.Notification.Shared;
using Zuhid.Notification.VerifyEmail;
using Zuhid.Notification.VerifyPhone;

namespace Zuhid.Notification.Controllers;

[AllowAnonymous]
[ApiController]
[Route("[controller]")]
public class IdentityController(NotificationQueue emailQueue) : ControllerBase
{
    [HttpPost("VerifyEmail")]
    public async Task VerifyEmail([FromBody] VerifyEmailMessage message) => await emailQueue.QueueMessage(message);

    [HttpPost("VerifyPhone")]
    public async Task VerifyPhone([FromBody] VerifyPhoneMessage message) => await emailQueue.QueueMessage(message);
}
