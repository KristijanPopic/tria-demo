using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TriaDemo.Repository.EntityFrameworkCore;
using TriaDemo.RestApi.Authorization;
using TriaDemo.RestApi.DependencyInjection;
using TriaDemo.RestApi.Exceptions;
using TriaDemo.Service.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails(o =>
    o.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    });

builder.Services.AddJwtBearerAuthentication(builder.Configuration);
builder.Services.AddScoped<IAuthorizationHandler, GroupRequirementAuthorizationHandler>();
builder.Services.AddAuthorization(
    policy =>
    {
        policy.AddPolicy("IsAdmin", p => p.AddRequirements(new GroupRequirement(Group.GroupAdmin)));
    });

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

app.UseAuthentication();
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