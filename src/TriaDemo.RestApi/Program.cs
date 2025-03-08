using Microsoft.EntityFrameworkCore;
using TriaDemo.Repository;
using TriaDemo.RestApi.DependencyInjection;
using TriaDemo.RestApi.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails(o =>
    o.CustomizeProblemDetails = context =>
    {
        context.ProblemDetails.Instance = $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
    });
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddTriaDemoServices(builder.Configuration);

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(
    o =>
    {
        o.SwaggerEndpoint("/openapi/v1.json", "Tria Demo API");
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
        catch(Exception ex)
        {
            retryCount++;
            if (retryCount == 10) throw;
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }
    }
}