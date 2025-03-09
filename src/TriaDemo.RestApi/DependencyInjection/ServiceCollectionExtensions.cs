using FluentValidation;
using TriaDemo.Repository.DependencyInjection;
using TriaDemo.RestApi.Controllers.ApiModels;
using TriaDemo.Service.DependencyInjection;

namespace TriaDemo.RestApi.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTriaDemoServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServiceLayer();
        services.AddRepositoryLayer(configuration.GetConnectionString("Database")!);
        
        return services;
    }
    
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        return services;
    }
}