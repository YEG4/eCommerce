using System.Text;
using eCommerce.API.Extensions;
using eCommerce.Core.Entities.Identity;
using eCommerce.Core.JsonObjects;
using eCommerce.Core.Services;
using eCommerce.Repository.Data;
using eCommerce.Repository.Identity;
using eCommerce.Repository.Identity.DataSeed;
using eCommerce.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

#region Services
builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaulConnectionString")));
builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityDbConnection"));
});
var jwtOptions = builder.Configuration.GetSection("JWT").Get<JwtOptions>();
builder.Services.AddIdentityServices();
builder.Services.AddSingleton(jwtOptions);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtOptions.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey))
                    };
                });
builder.Services.AddScoped<ITokenServices, TokenService>();
builder.Services.AddApplicationServices();

builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("RedisConnection");

    return ConnectionMultiplexer.Connect(connectionString);
});

#endregion
var app = builder.Build();

#region Migrations
using var scopedContainer = app.Services.CreateScope();
var services = scopedContainer.ServiceProvider;

var dbContext = services.GetRequiredService<StoreContext>();
var identityDbContext = services.GetRequiredService<AppIdentityDbContext>();
var factoryLogger = services.GetRequiredService<ILoggerFactory>();
var userManager = services.GetRequiredService<UserManager<AppUser>>();

try
{
    await dbContext.Database.MigrateAsync();
    await identityDbContext.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(dbContext);
    await IdentityDbContextSeed.UsersSeedAsync(userManager);
}
catch (Exception ex)
{
    var logger = factoryLogger.CreateLogger<Program>();
    logger.LogError(ex, "Error Occured While Migrating to database.");
}
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();