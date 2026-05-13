using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Zuhid.Base.Tests;

public class WebApplicationExtensionTests
{
    // [Fact]
    // public void AddServices_ShouldRegisterExpectedServices()
    // {
    //     // Arrange
    //     var args = Array.Empty<string>();
    //
    //     // Act
    //     var builder = WebApplicationExtension.AddServices(args);
    //
    //     // Assert
    //     Assert.NotNull(builder);
    //     var services = builder.Services;
    //
    //     // Check if Controllers are added (IActionDescriptorCollectionProvider is a good indicator)
    //     Assert.Contains(services, s => s.ServiceType == typeof(IActionDescriptorCollectionProvider));
    //
    //     // Check if Swagger is added
    //     Assert.Contains(services, s => s.ServiceType == typeof(ISwaggerProvider));
    //
    //     // Check if Cors is added
    //     Assert.Contains(services, s => s.ServiceType == typeof(ICorsService));
    // }
    //
    // [Fact]
    // public void BuildServices_ShouldReturnWebApplication()
    // {
    //     // Arrange
    //     var builder = WebApplicationExtension.AddServices([]);
    //
    //     // Act
    //     var app = builder.BuildServices();
    //
    //     // Assert
    //     Assert.NotNull(app);
    // }

    [Fact]
    public void AddPostgres_ShouldRegisterDbContext()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();
        // Using a simple Npgsql connection string format
        var connectionString = "Host=localhost;Database=testdb;Username=postgres;Password=password";

        // Act
        builder.AddPostgres<TestDbContext>(connectionString);

        // Assert
        var services = builder.Services;
        Assert.Contains(services, s => s.ServiceType == typeof(TestDbContext));

        var serviceProvider = services.BuildServiceProvider();
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<TestDbContext>();
        Assert.NotNull(dbContext);
    }
}
