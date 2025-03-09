using FluentValidation;
using TriaDemo.Repository.DependencyInjection;
using TriaDemo.RestApi.Controllers.ApiModels;
using TriaDemo.Service.DependencyInjection;

namespace TriaDemo.RestApi.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTriaDemoServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<TokenGenerator>();
        services.AddValidators();
        
        services.AddServiceLayer();
        services.AddRepositoryLayer(configuration.GetConnectionString("Database")!);

        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        services.AddSingleton<IValidator<UserLoginRequest>, UserLoginRequestValidator>();
        
        return services;
    }
}