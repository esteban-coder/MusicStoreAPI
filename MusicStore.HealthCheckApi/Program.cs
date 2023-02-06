using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using MusicStore.DataAccess;
using MusicStore.HealthCheckApi.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddDbContext<MusicStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "api" })
    .AddTypeActivatedCheck<PingHealthCheck>("Google", HealthStatus.Degraded, tags: new[] { "externo" }, "google.com" )
    .AddTypeActivatedCheck<PingHealthCheck>("Azure", HealthStatus.Degraded, tags: new[] { "externo" }, "azure.com" )
    .AddTypeActivatedCheck<PingHealthCheck>("Tienda", HealthStatus.Degraded, tags: new[] { "externo" }, "mercadolibre.com" )
    .AddDbContextCheck<MusicStoreDbContext>("Database", HealthStatus.Unhealthy, tags: new[] { "database" });

builder.Services.AddHealthChecksUI().AddInMemoryStorage();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseHttpsRedirection();

app.UseHealthChecksUI();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });
    
    endpoints.MapHealthChecks("/health/db", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        Predicate = x => x.Tags.Contains("database")
    });
    
    endpoints.MapHealthChecks("/health/external", new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        Predicate = x => x.Tags.Contains("externo")
    });
});

app.Run();
