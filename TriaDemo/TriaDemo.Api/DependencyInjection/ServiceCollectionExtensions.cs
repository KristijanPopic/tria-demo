using TriaDemo.Repository.DependencyInjection;
using TriaDemo.Service.DependencyInjection;

namespace TriaDemo.Api.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTriaDemoServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceLayer();
        services.AddRepositoryLayer(configuration.GetConnectionString("Database")!);
        
        return services;
    }
}