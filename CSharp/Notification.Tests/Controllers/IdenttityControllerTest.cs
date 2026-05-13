using Moq;
using Zuhid.Notification.Controllers;
using Zuhid.Notification.Shared;
using Zuhid.Notification.VerifyEmail;

namespace Zuhid.Notification.Tests.Controllers;

public class IdenttityControllerTest
{
    [Fact]
    public async Task Identtity_ShouldQueueMessage()
    {
        // Arrange
        var mockQueue = new Mock<NotificationQueue>();
        var controller = new IdentityController(mockQueue.Object);
        var message = new VerifyEmailMessage { AppUrl = "AppUrl" };

        // Act
        await controller.VerifyEmail(message);

        // Assert
        mockQueue.Verify(q => q.QueueMessage(It.Is<VerifyEmailMessage>(m => m.AppUrl == message.AppUrl)), Times.Once);
    }
}
