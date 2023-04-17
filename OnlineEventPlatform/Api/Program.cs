using Api;
using Domain.Identity.Entities;
using Infrastructure.Database;
using Infrastructure.Security;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using NSwag;
using NSwag.Generation.Processors.Security;

var config = GetConfiguration();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

builder.Services.AddJwt();

builder.Services.AddControllers();

builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "Online event platform backend documentation";
    document.AddSecurity("Bearer", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        Description = "Paste your JWT token into the input field.",
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header
    });

    document.OperationProcessors.Add(
        new AspNetCoreOperationSecurityScopeProcessor("Bearer"));
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddServices();

builder.Services.AddDbContextFactory<OnlineEventContext>(opts => opts.UseSqlServer(config["ConnectionString"]));
builder.Services.AddScoped<IDbContextWrapper<OnlineEventContext>, DbContextWrapper<OnlineEventContext>>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        builder => builder
            .SetIsOriginAllowed((host) => true)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

MigrateDatabase(app);

// Configure the HTTP request pipeline.
app.UseCors(builder =>
    builder
        .WithOrigins()
        .AllowCredentials()
        .AllowAnyHeader()
        .AllowAnyMethod());

// Configure the HTTP request pipeline.
app.UseStatusCodePages();
app.UseOpenApi();
app.UseSwaggerUi3(config => config.TransformToExternalPath = (internalUiRoute, request) =>
{
    if (internalUiRoute.StartsWith("/") == true &&
        internalUiRoute.StartsWith(request.PathBase) == false)
    {
        return request.PathBase + internalUiRoute;
    }
    else
    {
        return internalUiRoute;
    }
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.MapControllers();

app.Run();

IConfiguration GetConfiguration()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables();

    return builder.Build();
}

void MigrateDatabase(IHost host)
{
    using (var serviceScope = host.Services.CreateScope())
    {
        var services = serviceScope.ServiceProvider;

        try
        {
            var context = services.GetService<OnlineEventContext>();
            var passwordHasher = services.GetService<IPasswordHasher>();

            if (context != null)
            {
                context.Database.Migrate();

                var email = "a.y.senokosov@student.khai.edu";

                var password = config.GetValue<string>("Password");

                var user = context.Users
                    .SingleOrDefault(u => u.Email == email && !u.IsDeleted);

                if (user == null && passwordHasher != null && password != null)
                {
                    var salt = Guid.NewGuid().ToByteArray();
                    {
                        var person = new User()
                        {
                            Email = email,
                            Hash = passwordHasher.Hash(password, salt),
                            Salt = salt
                        };

                        context.Users.Add(person);
                        context.SaveChanges();
                    }
                }
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}