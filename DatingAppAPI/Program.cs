using DatingAppAPI.Data;
using DatingAppAPI.Entities;
using DatingAppAPI.Extensions;
using DatingAppAPI.Middleware;
using DatingAppAPI.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
.WithOrigins("http://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
	var context = services.GetRequiredService<DataContext>();
	var userManager = services.GetRequiredService<UserManager<AppUser>>();
	var roleManager = services.GetRequiredService<RoleManager<AppRole>>();

	await context.Database.MigrateAsync();
	//context.Connections.RemoveRange(context.Connections);
	//await context.Database.ExecuteSqlRawAsync("DELETE * FROM TABLE [Connections]");

	await Seed.SeedUsers(userManager, roleManager);
}
catch (Exception ex)
{
	var logger = services.GetService<ILogger<Program>>();
	logger.LogError(ex, "An error occured during the migration");
}

app.Run();
