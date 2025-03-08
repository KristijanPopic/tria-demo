using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TriaDemo.Service.Models;
using TriaDemo.Service.Validators;

namespace TriaDemo.Service.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddValidators();
        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<IValidator<User>, UserValidator>();
        return services;
    }
}