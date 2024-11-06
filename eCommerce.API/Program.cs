using eCommerce.API.Extensions;
using eCommerce.Repository.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

#region Services
builder.Services.AddControllers();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StoreContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaulConnectionString")));
builder.Services.AddApplicationServices();
#endregion
var app = builder.Build();

#region Migrations
using var scopedContainer = app.Services.CreateScope();
var services = scopedContainer.ServiceProvider;

var dbContext = services.GetRequiredService<StoreContext>();
var factoryLogger = services.GetRequiredService<ILoggerFactory>();

try
{
    await dbContext.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(dbContext);
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

app.MapControllers();

app.Run();