using AutoMapper;
using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Interaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly ITokenService _tokenService;
		private readonly IMapper _mapper;

		public AccountController(UserManager<AppUser> userManager, //Was Datacontext context
			ITokenService tokenService,
			IMapper mapper)
		{
			_userManager = userManager;
			_tokenService = tokenService;
			this._mapper = mapper;
		}

		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			if (await UserExists(registerDto.Username))
			{
				return BadRequest("Username is taken");
			}

			var user = _mapper.Map<AppUser>(registerDto);

			//using var hmac = new HMACSHA512();


			user.UserName = registerDto.Username.ToLower();
			//user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
			//user.PasswordSalt = hmac.Key;


			var result = await _userManager.CreateAsync(user, registerDto.Password);

			if (!result.Succeeded)
			{
				return BadRequest(result.Errors);
			}
			//_context.Users.Add(user);
			//await _context.SaveChangesAsync();

			var roleResult = await _userManager.AddToRoleAsync(user, "Member");

			if (!roleResult.Succeeded)
			{
				return BadRequest(result.Errors);
			}

			return new UserDto
			{
				Username = user.UserName,
				Token = await _tokenService.CreateToken(user),
				PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
				KnownAs = user.KnownAs,
				Gender = user.Gender,
			};

		}


		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			//Console.WriteLine("Logging in");
			var user = await _userManager.Users.Include(p => p.Photos)
				.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
			if (user == null)
			{
				return Unauthorized("Invalid Username"); //Cannot return Unauthorized alone with Task because it is HttpResult and not Task. Must
														 //wrap Task in IActionResult;
			}

			var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

			if (!result)
			{
				return Unauthorized("Invalid Password");
			}
			//Attempting to get back PasswordHash
			//Using key to get back exact hash created when we passed in password
			//using var hmac = new HMACSHA512(user.PasswordSalt); // <-- This return a byte array
			//Is the password is the same as the one in the database, then our hashing algorithm will be identical
			//var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

			//for (int i = 0; i < computedHash.Length; i++)
			//{
			//	if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
			//}

			return new UserDto
			{
				Username = user.UserName,
				Token = await _tokenService.CreateToken(user),
				PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain)?.Url,
				KnownAs = user.KnownAs,
				Gender = user.Gender
			};


		}

		private async Task<bool> UserExists(string username)
		{
			return await _userManager.Users.AnyAsync(user => user.UserName == username.ToLower());
		}

	}
}
