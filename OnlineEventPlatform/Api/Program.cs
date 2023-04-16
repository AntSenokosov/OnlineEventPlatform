using Api;
using Infrastructure.Database;
using Infrastructure.Services;
using Infrastructure.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

var config = GetConfiguration();
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddOpenApiDocument(document =>
{
    document.Title = "Online event platform backend documentation";
});

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

            if (context != null)
            {
                context.Database.Migrate();
            }
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }
}