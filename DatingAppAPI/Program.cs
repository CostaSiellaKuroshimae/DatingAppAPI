using DatingAppAPI.Data;
using DatingAppAPI.Extentions;
using DatingAppAPI.Interaces;
using DatingAppAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseCors(corsPolicyBuilder => corsPolicyBuilder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
