using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Zuhid.Base;
using Zuhid.Identity.Controllers;
using Zuhid.Identity.Entities;
using Zuhid.Identity.NotificationClients;
using Zuhid.Identity.Repositories;
using Zuhid.Identity.Requests;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Zuhid.Identity.Validators;
using Zuhid.Identity.Providers;

namespace Zuhid.Identity.Tests;

public class AccountControllerTests
{
    private readonly Mock<UserRepository> _mockUserRepository;
    private readonly Mock<NotificationClient> _mockNotificationClient;
    private readonly Mock<JwtProvider> _mockJwtProvider;
    private readonly AccountController _controller;

    public AccountControllerTests()
    {
        var mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(), null!, null!, null!, null!, null!, null!, null!, null!);
        var mockSignInManager = new Mock<SignInManager<User>>(mockUserManager.Object, Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(), Mock.Of<IUserClaimsPrincipalFactory<User>>(), null!, null!, null!, null!);

        _mockUserRepository = new Mock<UserRepository>(mockUserManager.Object, mockSignInManager.Object);

        var inMemorySettings = new Dictionary<string, string?> {
            {"ConnectionStrings:Identity", "Host=localhost;Database=Identity;Username=postgres;Password=[postgres_credential]"},
            {"ConnectionStrings:Log", "Host=localhost;Database=Log;Username=postgres;Password=[postgres_credential]"},
            {"postgres_credential", "testpassword"}
        };
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var appSetting = new AppSetting(configuration)
        {
            Notification = new AppSetting.NotificationOptions { BaseUrl = "http://localhost", Authorization = "test" },
            Jwt = new BaseSetting.JwtOptions
            {
                Issuer = "TestIssuer",
                Audience = "TestAudience",
                ExpiryInMinutes = 60,
                PrivateKeyPath = "Keys/private.key",
                PublicKeyPath = "Keys/public.key"
            }
        };

        _mockNotificationClient = new Mock<NotificationClient>(new HttpClient(), appSetting, Mock.Of<ILogger<NotificationClient>>());
        _mockJwtProvider = new Mock<JwtProvider>(appSetting);

        _controller = new AccountController(_mockUserRepository.Object, _mockNotificationClient.Object, appSetting, new LoginValidator(), _mockJwtProvider.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(new ClaimsIdentity()) }
            }
        };
    }

    [Fact]
    public async Task Login_ShouldReturnToken_WhenLoginSucceeds()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "Password123!");
        var user = new User { Email = request.Email, Id = Guid.NewGuid() };
        _mockUserRepository.Setup(r => r.Login(request.Email, request.Password))
            .ReturnsAsync((Microsoft.AspNetCore.Identity.SignInResult.Success, user));

        _mockJwtProvider.Setup(p => p.GenerateToken(user))
            .Returns("test-token");

        // Act
        var result = await _controller.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = okResult.Value;
        Assert.NotNull(response);
        var tokenProperty = response.GetType().GetProperty("Token");
        Assert.NotNull(tokenProperty);
        Assert.Equal("test-token", tokenProperty.GetValue(response));
    }

    [Fact]
    public async Task Login_ShouldReturnBadRequest_WhenLoginFails()
    {
        // Arrange
        var request = new LoginRequest("test@example.com", "WrongPassword");
        _mockUserRepository.Setup(r => r.Login(request.Email, request.Password))
            .ReturnsAsync((Microsoft.AspNetCore.Identity.SignInResult.Failed, (User?)null));

        // Act
        var result = await _controller.Login(request);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result);
        var modelState = Assert.IsType<SerializableError>(badRequest.Value);
        Assert.True(modelState.ContainsKey("Login"));
    }

    [Fact]
    public async Task Get_ShouldReturnOk_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Email = "test@example.com" };
        _mockUserRepository.Setup(r => r.GetById(userId))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.Get(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(userId, returnedUser.Id);
    }

    [Fact]
    public async Task Get_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        // Arrange
        var userId = Guid.NewGuid();
        _mockUserRepository.Setup(r => r.GetById(userId))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.Get(userId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
