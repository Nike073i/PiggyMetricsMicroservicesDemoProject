using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StatisticsService.Domain;
using StatisticsService.Domain.Timeseries;
using StatisticsService.Services;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace StatisticsService.Controllers
{
    [Route("statistics")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("current")]
        public async Task<List<DataPoint>> GetCurrentAccountStatistics()
        {
            return await _statisticsService.FindByAccountName(User.Identity.Name);
        }

        [HttpGet("{accountName}")]
        //[Authorize(Policy = "server, demo")]
        public async Task<List<DataPoint>> GetStatisticsByAccountName([FromRoute] string accountName)
        {
            return await _statisticsService.FindByAccountName(accountName);
        }

        [HttpPut("{accountName}")]
        //[Authorize(Policy = "server")]
        public async Task SaveAccountStatistics([FromRoute] string accountName, [Required][FromBody] Account account)
        {
            await _statisticsService.Save(accountName, account);
        }
    }
}
