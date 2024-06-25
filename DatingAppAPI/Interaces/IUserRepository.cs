using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Helpers;

namespace DatingAppAPI.Interaces
{
	public interface IUserRepository
	{
		void Update(AppUser user);
		Task<IEnumerable<AppUser>> GetUsersAsync();
		Task<AppUser> GetUserByIdAsync(int id);
		Task<AppUser> GetUserByUsernameAsync(string username);

		Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);

		Task<MemberDto> GetMemberAsync(string username);
		Task<string> GetUserGender(string username);

	}
}
