using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;

namespace Zuhid.Base;

public static class WebApplicationExtension
{
    public static void AddServices<TSetting>(this WebApplicationBuilder builder, TSetting setting, string[] scopedSuffixes) where TSetting : BaseSetting
    {
        builder.Services.AddSingleton(setting);
        // builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        builder.Services.AddAuthentication(option =>
         {
             option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
             option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
         })
            .AddJwtBearer(options =>
            {
                var rsa = RSA.Create();
                rsa.ImportFromPem(File.ReadAllText(setting.Jwt.PublicKeyPath));
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = setting.Jwt.Issuer,
                    ValidAudience = setting.Jwt.Audience,
                    IssuerSigningKey = new RsaSecurityKey(rsa)
                };

                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("JwtBearer");
                        logger.LogInformation("Authentication validated for token. Request Path: {Path}", context.HttpContext.Request.Path);
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("JwtBearer");
                        logger.LogError(context.Exception, "Authentication failed for token. Request Path: {Path}", context.HttpContext.Request.Path);
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("JwtBearer");
                        logger.LogWarning("JWT Challenge: {Error}, {ErrorDescription}. AuthenticateFailure: {Failure}", context.Error, context.ErrorDescription, context.AuthenticateFailure?.Message);
                        return Task.CompletedTask;
                    },
                    OnForbidden = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("JwtBearer");
                        logger.LogWarning("JWT Forbidden for user: {User}", context.HttpContext.User.Identity?.Name);
                        return Task.CompletedTask;
                    }
                };
            });
        builder.Services.AddAuthorization();
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ActionFilter>();
            options.Filters.Add<ExceptionFilter>();
        });
        builder.Services.AddCors(options => options.AddPolicy(name: "CorsPolicy", policy => policy
        // .WithOrigins(appSettings.CorsOrigin)
          .AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader()
        ));

        // This will add all classes that end with Repository, Mapper, and Validator to the service collection
        builder.Services.AddScoped<HttpClient>();
        Assembly.GetCallingAssembly().GetTypes().Where(s =>
            s.IsClass
            && scopedSuffixes.Any(suffix => s.Name.EndsWith(suffix))
          )
          .ToList()
          .ForEach(item => builder.Services.AddScoped(item));

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = setting.Name, Version = setting.Version });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                Name = "Authorization",
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
            });
            c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });
    }

    public static WebApplication BuildServices(this WebApplicationBuilder builder, BaseSetting setting)
    {
        var app = builder.Build();
        app.UseCors("CorsPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint($"/swagger/{setting.Version}/swagger.json", $"{setting.Name} {setting.Version}");
            c.RoutePrefix = "swagger"; // Set Swagger UI at /swagger
        });
        app.MapGet("/", async context => await context.Response.WriteAsync("""
    <html>
    <body style='padding:100px 0;text-align:center;font-size:xxx-large;'>
        <a href='./swagger/index.html'>View Swagger</a>
    </body>
    </html>
    """));
        app.MapControllers();
        return app;
    }

    public static void AddPostgres<TContext>(this WebApplicationBuilder builder, string connectionString) where TContext : DbContext
    {
        builder.Services.AddDbContext<TContext>(options => options
          .UseNpgsql(connectionString)
          .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking) // setting to no tracking to improve performance
          .EnableSensitiveDataLogging(builder.Environment.IsDevelopment())
        );
    }
}

