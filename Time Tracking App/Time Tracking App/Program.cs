using Microsoft.EntityFrameworkCore;
using TimeTracking.Infrastructure;
using MediatR;
using AutoMapper.Internal;
using TimeTracking.Application.Activities;
using TimeTracking.Core.Repositories;
using TimeTracking.Infrastructure.Repositories;
using Time_Tracking_App.Mapper;
using Time_Tracking_App.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddAutoMapper(cfg => cfg.Internal().MethodMappingEnabled = false, typeof(MappingProfiles).Assembly);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ConfigureServices(builder.Services, builder.Configuration);
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ActivityById.Handler).Assembly));

//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ActivityById.Handler).Assembly));



var app = builder.Build();

await ApplyDbMigrationsAndSeedDataAsync(app.Services);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(policyBuilder =>
    policyBuilder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials()
        .WithOrigins("http://localhost:5001", "https://localhost:5001")
);

app.UseAuthorization();

app.MapControllers();

app.Run();

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    ConfigureDatabase(services, configuration);
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped<ActivityService>();  
    services.AddHttpClient();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo { Title = "TimeTracking API", Version = "v1" });
        var xmlFile = Path.Combine(AppContext.BaseDirectory, "TimeTrackingDocumentation.xml");
        options.IncludeXmlComments(xmlFile);
    });
}

void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
{
    // use in-memory database
    services.AddDbContext<TimeTrackingContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("TimeTrackingServer")));
}

async Task ApplyDbMigrationsAndSeedDataAsync(IServiceProvider services)
{
    using var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    await using var context = scope.ServiceProvider.GetRequiredService<TimeTrackingContext>();
    await context.Database.MigrateAsync();
    await SeedHelper.SeedData(context);
}