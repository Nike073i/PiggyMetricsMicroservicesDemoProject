using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Domain;
using NotificationService.Services;

namespace NotificationService.Controllers
{
    [Authorize]
    [Route("recipients")]
    [ApiController]
    public class RecipientController : ControllerBase
    {
        private readonly IRecipientService _recipientService;

        public RecipientController(IRecipientService service)
        {
            _recipientService = service;
        }
        
        [HttpGet("current")]
        public async Task<Recipient> GetCurrentNotificationSettings()
        {
            return await _recipientService.FindRecipientAsync(User.Identity.Name,
                HttpContext.RequestAborted);
        }

        [HttpPut("current")]
        public async Task<Recipient> SaveCurrentNotificationSetting([FromBody] [Required] Recipient recipient)
        {
            return await _recipientService.SaveAsync(User.Identity.Name ,
                recipient, HttpContext.RequestAborted);
        }
    }
}