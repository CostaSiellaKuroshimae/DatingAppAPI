using DatingAppAPI.Entities;

namespace DatingAppAPI.Interaces
{
	public interface ITokenService
	{
		Task<string> CreateToken(AppUser user);
	}
}
