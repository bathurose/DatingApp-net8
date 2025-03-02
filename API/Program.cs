using API.Data;
using API.Entities;
using API.Extensions;
using API.Middleware;
using API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();
app.UseMiddleware<ExceptionMiddleware>();
// Configure the HTTP request pipeline.
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
                    .AllowCredentials() //signalR
                    .WithOrigins("http://localhost:4200", "https://localhost:4200")) ;

app.UseHttpsRedirection();

app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllers();

app.MapHub<PresenceHub>("hubs/precense"); //signalR
app.MapHub<MessageHub>("hubs/message"); //signalR
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
   
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]"); //sqlserver
   /* await context.Database.ExecuteSqlRawAsync("DELETE FROM \"Connections\"");*///postgres

    await Seed.SeedUsers(userManager,roleManager);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occur during migration");
}

app.Run();
