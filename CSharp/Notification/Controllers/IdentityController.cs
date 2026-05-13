using Microsoft.AspNetCore.Mvc;
using Zuhid.Notification.Shared;
using Zuhid.Notification.VerifyEmail;

namespace Zuhid.Notification.Controllers;

[ApiController]
[Route("[controller]")]
public class IdentityController(NotificationQueue emailQueue) : ControllerBase
{
    [HttpPost("VerifyEmail")]
    public async Task VerifyEmail([FromBody] VerifyEmailMessage message) => await emailQueue.QueueMessage(message);

}
