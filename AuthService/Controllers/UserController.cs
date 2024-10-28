using AuthService.Domain;
using AuthService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AuthService.Controllers
{
	[Route("users")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserManager _userManager;

		public UserController(IUserManager userManager)
		{
			_userManager = userManager;
		}

		[HttpGet("current")]
		public ClaimsPrincipal GetUser(ClaimsPrincipal principal)
		{
			return principal;
		}

		[HttpPost]
		//[Authorize(Policy = "server")]
		public async Task CreateUser([FromBody] UserModel user, CancellationToken token)
		{
			await _userManager.CreateAsync(user, token);
		}
	}
}
