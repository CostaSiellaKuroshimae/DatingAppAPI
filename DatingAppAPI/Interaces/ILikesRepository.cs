using DatingAppAPI.DTOs;
using DatingAppAPI.Entities;
using DatingAppAPI.Helpers;

namespace DatingAppAPI.Interaces
{
	public interface ILikesRepository
	{
		Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
		Task<AppUser> GetUserWithLikes(int userId);
		Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams);

	}
}
