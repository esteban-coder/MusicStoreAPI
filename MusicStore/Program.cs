using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MusicStore.DataAccess;
using MusicStore.Entities;
using MusicStore.Services.Extensions;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using System.Text;
using MusicStore.DatabaseFirst;

var builder = WebApplication.CreateBuilder(args);

var corsConfig = "MusicStoreApi";

var logger = new LoggerConfiguration()
    .WriteTo.Console(restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information)
    //.WriteTo.File("..\\log.log",
    //    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}",
    //    rollingInterval: RollingInterval.Day,
    //    restrictedToMinimumLevel: LogEventLevel.Error)
    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("Default"), 
        new MSSqlServerSinkOptions
        {
            AutoCreateSqlTable = true,
            TableName = "ApiLogs"
        }, restrictedToMinimumLevel: LogEventLevel.Information)
    .CreateLogger();

builder.Host.ConfigureLogging(options =>
{
    //options.ClearProviders();
    options.AddSerilog(logger);
});


builder.Services.AddCors(setup =>
{
    setup.AddPolicy(corsConfig, x =>
    {
        x.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Configure serialization json depth
//builder.Services.AddControllers().AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.MaxDepth = 1;
//});

// Mapeamos el archivo appsettings{environment}.json en una clase de tipo AppSettings
builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddDbContext<MusicStoreDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));

    
    // Con esto desactivamos el ChangeTracker para el objeto DbContext
    //options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});


// PARA MIS PRUEBAS DE DATABASE FIRST
builder.Services.AddDbContext<MusicDbFirstContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<MusicStoreUserIdentity, IdentityRole>(policies =>
    {
        policies.Password.RequireDigit = false;
        policies.Password.RequiredLength = 6;
        policies.Password.RequireLowercase = false;
        policies.Password.RequireNonAlphanumeric = false;
        policies.Password.RequireUppercase = false;

        policies.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<MusicStoreDbContext>()
    .AddDefaultTokenProviders();


builder.Services.AddDependencies()
    .AddFileUploader(builder.Environment.IsDevelopment())
    .AddAutoMapperConfigurations(); 

// Crear Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole(Constants.RoleAdmin));
    options.AddPolicy("User", policy => policy.RequireRole(Constants.RoleCustomer));

    //options.AddPolicy("Admin", policy => policy.RequireClaim(ClaimTypes.Email, "gmail", "hotmail.com"));
});

builder.Services.AddAuthentication(x =>
{
    x.DefaultChallengeScheme = "Bearer";
    x.DefaultAuthenticateScheme = "Bearer";
}).AddJwtBearer(x =>
{
    var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"]);

    x.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(corsConfig);

app.UseHttpsRedirection();
app.MapControllers();

app.MapGet("/api/databasefirst/genres", async (MusicDbFirstContext context) => Results.Ok(await context.Genres
    .AsNoTracking()
    .Select(p => new
    {
        p.Id,
        Genero = p.Name,
        Comentarios = p.Comments
    })
    .ToListAsync()));

//app.MapGet("/api/Genres", async (MusicStoreDbContext context) => await context.Set<Genre>().ToListAsync());

//app.MapGet("/api/Genres/{id:int}", async (MusicStoreDbContext context, int id) =>
//{
//    var genre = await context.Set<Genre>()
//        .Select(p => new
//        {
//            p.Id,
//            p.Name
//        })
//        .FirstOrDefaultAsync(p => p.Id == id);

//    if (genre is null)
//    {
//        return Results.NotFound();
//    }

//    return Results.Ok(new
//    {
//        IdGenre = genre.Id,
//        genre.Name,
//    });
//}).Produces<Genre>(StatusCodes.Status200OK)
//    .Produces(StatusCodes.Status404NotFound);

//app.MapPost("api/Genres", (Genre person) =>
//{
//    list.Add(person);

//    return Results.Created($"/api/Genres/{person.Id}", person);
//});

//app.MapPut("api/Genres/{id:int}", (int id, Genre person) => list[id] = person);

//app.MapDelete("api/Genres/{id:int}", (int id) =>
//{
//    var person = list.FirstOrDefault(x => x.Id == id);
//    if (person == null)
//    {
//        return Results.NotFound();
//    }

//    list.Remove(person);

//    return Results.Ok();
//});

app.Run();
