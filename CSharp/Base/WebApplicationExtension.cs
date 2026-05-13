using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using System.Reflection;

namespace Zuhid.Base;

public static class WebApplicationExtension
{
    public static (WebApplicationBuilder, TSetting) AddServices<TSetting>(string[] args, string[] scopedSuffixes) where TSetting : BaseSetting, new()
    {
        var builder = WebApplication.CreateBuilder(args);
        var setting = new TSetting();
        setting.Bind(builder.Configuration);

        builder.Services.AddSingleton(setting);
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<ActionFilter>();
            options.Filters.Add<ExceptionFilter>();

        });
        builder.Services.AddCors(options => options.AddPolicy(name: "CorsPolicy", policy => policy
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
        return (builder, setting);
    }

    public static WebApplication BuildServices(this WebApplicationBuilder builder, BaseSetting setting)
    {
        var app = builder.Build();
        app.UseCors("CorsPolicy");
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

