using System.Text;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using TriaDemo.Repository.DependencyInjection;
using TriaDemo.RestApi.Controllers.ApiModels;
using TriaDemo.RestApi.Options;
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

    public static IServiceCollection AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptions = new JwtTokenOptions();
        configuration.GetSection(JwtTokenOptions.SectionName).Bind(jwtOptions);

        services.Configure<JwtTokenOptions>(configuration.GetSection(JwtTokenOptions.SectionName));
        
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(
                x =>
                {
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey!)),
                        ValidIssuer = jwtOptions.Issuer,
                        ValidAudience = jwtOptions.Audience,
                    };
                }
            );
        return services;
    }

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(
            option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        []
                    }
                });
            });
        
        return services;
    }

    private static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddSingleton<IValidator<CreateUserRequest>, CreateUserRequestValidator>();
        services.AddSingleton<IValidator<UserLoginRequest>, UserLoginRequestValidator>();
        
        return services;
    }
}