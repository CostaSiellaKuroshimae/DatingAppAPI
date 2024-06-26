﻿using DatingAppAPI.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DatingAppAPI.Data
{
	public class Seed
	{
		public static async Task SeedUsers(UserManager<AppUser> userManager,
			RoleManager<AppRole> roleManager)  //Was DataContext context
		{
			if (await userManager.Users.AnyAsync())
			{
				return;
			}

			var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

			var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

			var roles = new List<AppRole>
			{
				new AppRole{Name = "Member"},
				new AppRole{Name = "Admin"},
				new AppRole{Name = "Moderator"},

			};

			foreach (var role in roles)
			{
				await roleManager.CreateAsync(role);
			}

			foreach (var user in users)
			{
				//using var hmac = new HMACSHA512();

				user.UserName = user.UserName.ToLower();
				//user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd")); //Password for seeded users is Pa$$w0rd
				//user.PasswordSalt = hmac.Key;

				await userManager.CreateAsync(user, "Pa$$w0rd");

				await userManager.AddToRoleAsync(user, "Member");
			}

			var admin = new AppUser
			{
				UserName = "admin",
			};

			await userManager.CreateAsync(admin, "Pa$$w0rd");

			await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
			//await context.SaveChangesAsync(); UserManager.CreateAsync creates and saves
		}
	}
}
