using Microsoft.AspNetCore.Identity;

namespace DatingAppAPI.Entities
{
	public class AppUserRole : IdentityUserRole<int> //Join table for AppUsers and Roles
	{
		public AppUser User { get; set; }
		public AppRole Role { get; set; }
	}
}
