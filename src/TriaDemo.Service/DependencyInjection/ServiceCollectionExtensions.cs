using Microsoft.Extensions.DependencyInjection;

namespace TriaDemo.Service.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<INotificationsService, NotificationsService>();
        services.AddScoped<CurrentUserService>();
        
        return services;
    }
}