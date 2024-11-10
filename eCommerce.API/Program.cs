using eCommerce.API.Extensions;
using eCommerce.Core.Entities.Identity;
using eCommerce.Repository.Data;
using eCommerce.Repository.Identity;
using eCommerce.Repository.Identity.DataSeed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddApplicationServices();
builder.Services.AddIdentityServices();

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