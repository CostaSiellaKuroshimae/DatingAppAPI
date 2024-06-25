using DatingAppAPI.Extensions;
using DatingAppAPI.Interaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingAppAPI.Helpers
{
	public class LogUserActivity : IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var resultContext = await next(); //using next allows the API action to take place and then we can do our logic
											  //use the context to do something before the API action takes place.
			if (!resultContext.HttpContext.User.Identity.IsAuthenticated) { return; }

			var userId = resultContext.HttpContext.User.GetUserId();

			var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
			var user = await uow.UserRepository.GetUserByIdAsync(userId);
			user.LastActive = DateTime.UtcNow;
			await uow.Complete();
		}
	}
}
