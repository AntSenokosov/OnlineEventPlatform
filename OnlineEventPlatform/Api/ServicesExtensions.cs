using Application.Catalog.Services;
using Application.Catalog.Services.Interfaces;
using Application.Catalog.Repositories;
using Application.Catalog.Repositories.Interfaces;

namespace Api;

public static class ServicesExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<IDepartmentRepository, DepartmentRepository>();
        services.AddTransient<IPositionRepository, PositionRepository>();
        services.AddTransient<ISpeakerRepository, SpeakerRepository>();
        services.AddTransient<IOnlineEventRepository, OnlineEventRepository>();
        services.AddTransient<IDepartmentService, DepartmentService>();
        services.AddTransient<IPositionService, PositionService>();
        services.AddTransient<ISpeakerService, SpeakerService>();
        services.AddTransient<IOnlineEventService, OnlineEventService>();
    }
}