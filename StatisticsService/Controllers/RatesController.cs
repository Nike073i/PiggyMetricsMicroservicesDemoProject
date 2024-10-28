using Microsoft.AspNetCore.Mvc;
using StatisticsService.Domain;
using StatisticsService.Services;
using System.Threading.Tasks;

namespace StatisticsService.Controllers
{
    [Route("rates")]
    [ApiController]
    public class RatesController : ControllerBase
    {
        private readonly IExchangeRatesService _exchangeRatesService;

        public RatesController(IExchangeRatesService exchangeRatesService)
        {
            _exchangeRatesService = exchangeRatesService;
        }

        [HttpGet("latest/{base}")]
        public async Task<ExchangeRatesContainer> Latest([FromRoute]Currency @base)
        {
            return await _exchangeRatesService.GetRatesContainer(@base);
        }
    }
}
