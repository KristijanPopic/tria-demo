using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TriaDemo.Repository;
using TriaDemo.RestApi.DependencyInjection;
using TriaDemo.RestApi.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
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

builder.Services.AddProblemDetails(o =>
    o.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    });
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddAuthorization();
builder.Services.AddJwtBearerAuthentication(builder.Configuration);

builder.Services.AddTriaDemoServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(
    o =>
    {
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    }
);
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var serviceScope = app.Services.CreateScope())
{
    InitializeDatabase(serviceScope);
}

await app.RunAsync();
return;

static void InitializeDatabase(IServiceScope serviceScope)
{
    using var dbContext = serviceScope.ServiceProvider.GetRequiredService<TriaDemoDbContext>();
    
    var migrated = false;
    var retryCount = 0;
    // sometimes the SQL Server doesn't get available immediately when starting with Docker compose
    // so the application crashes here. This ensures it will retry
    while (!migrated && retryCount < 10)
    {
        try
        {
            dbContext.Database.Migrate();
            migrated = true;
        }
        catch
        {
            retryCount++;
            if (retryCount == 10) throw;
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
    }
}