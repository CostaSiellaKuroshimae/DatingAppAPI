﻿using DatingAppAPI.Data;
using DatingAppAPI.Helpers;
using DatingAppAPI.Interaces;
using DatingAppAPI.Services;
using DatingAppAPI.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Extensions
{
	public static class ApplicationServiceExtentions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddDbContext<DataContext>(options =>
			{
				options.UseSqlite(config.GetConnectionString("DefaultConnection"));
			});

			services.AddCors();
			services.AddScoped<ITokenService, TokenService>();
			//services.AddScoped<IUserRepository, UserRepository>();
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
			services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
			services.AddScoped<IPhotoService, PhotoService>();
			services.AddScoped<LogUserActivity>();
			//services.AddScoped<ILikesRepository, LikesRepository>();
			//services.AddScoped<IMessageRepository, MessageRepository>();
			services.AddSignalR();
			services.AddSingleton<PresenceTracker>();
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			return services;
		}


	}
}
