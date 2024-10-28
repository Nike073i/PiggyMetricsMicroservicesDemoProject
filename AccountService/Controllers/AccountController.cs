using AccountService.Domain;
using AccountService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AccountService.Controllers
{
    [Route("accounts")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{name=demo}")]
        public async Task<Account> GetAccountByName([FromRoute] string name)
        {
            return await _accountService.FindByName(name);
        }

        [Authorize]
        [HttpGet("current")]
        public async Task<Account> GetCurrentAccount()
        {
            return await _accountService.FindByName(User.Identity.Name);
        }

        [Authorize]
        [HttpPut("current")]
        public async Task SaveCurrentAccount([FromBody] Account account)
        {
            await _accountService.SaveChanges(User.Identity.Name, account);
        }

        [HttpPost]
        public async Task<Account> CreateNewAccount([FromBody] User user)
        {
            return await _accountService.Create(user);
        }

        [Authorize]
        [HttpGet("test")]
        public async Task<IActionResult> Test()
        {
            return new JsonResult(new { Status = "OK"});
        }
    }
}
