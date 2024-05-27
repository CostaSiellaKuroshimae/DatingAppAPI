using DatingAppAPI.Data;
using DatingAppAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAppAPI.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _context;

        public BuggyController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "Secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var notFound = _context.Users.Find(-1);

            if (notFound == null)
            {
                return NotFound();
            }

            return notFound;
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var notFound = _context.Users.Find(-1);

            var returnObject = notFound.ToString();

            return returnObject;
        }

        [HttpGet("bad-request")]
        public ActionResult<AppUser> GetBadRequest()
        {
            return BadRequest("This was not a good request.");
        }
    }
}
