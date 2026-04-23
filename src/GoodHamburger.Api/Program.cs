using GoodHamburger.Application;
using GoodHamburger.Infrastructure;
using GoodHamburger.Infrastructure.Data;
using GoodHamburger.Api.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

const string CorsPolicyName = "BlazorFrontend";
var frontendBaseUrl = builder.Configuration["FrontendBaseUrl"];

if (!string.IsNullOrWhiteSpace(frontendBaseUrl))
{
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(CorsPolicyName, policy =>
        {
            policy
                .WithOrigins(frontendBaseUrl)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });
}

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    await DbSeeder.SeedAsync(dbContext, CancellationToken.None);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (!string.IsNullOrWhiteSpace(frontendBaseUrl))
{
    app.UseCors(CorsPolicyName);
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();
