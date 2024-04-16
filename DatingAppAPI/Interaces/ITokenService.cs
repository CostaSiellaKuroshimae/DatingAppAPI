using DatingAppAPI.Entities;

namespace DatingAppAPI.Interaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}
