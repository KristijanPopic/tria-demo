using Microsoft.EntityFrameworkCore;
using TriaDemo.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<TriaDemoDbContext>(opt =>
    opt.UseNpgsql(
        builder.Configuration.GetConnectionString("BloggingContext"),
        o => o
            .SetPostgresVersion(17, 4)
        ));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();