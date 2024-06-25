using DatingAppAPI.Entities;
using DatingAppAPI.Interaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingAppAPI.Services
{
	public class TokenService : ITokenService
	{
		private readonly SymmetricSecurityKey _symmetricSecurityKey;
		private readonly UserManager<AppUser> _userManager;

		public TokenService(IConfiguration config, UserManager<AppUser> userManager)
		{
			_symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
			_userManager = userManager;
		}


		public async Task<string> CreateToken(AppUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()), //was changed from user.UserName to User.Id
				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
			};

			var roles = await _userManager.GetRolesAsync(user);

			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			var creds = new SigningCredentials(_symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = creds
			};

			var tokenHandler = new JwtSecurityTokenHandler();
			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}




