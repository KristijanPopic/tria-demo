using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TriaDemo.Service.Contracts;

namespace TriaDemo.Repository.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositoryLayer(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<TriaDemoDbContext>(opt =>
            opt.UseNpgsql(connectionString));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        
        return services;
    }
}