using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Yanyana.BackEnd.Business.Managers;
using Yanyana.BackEnd.Core.Entities;

namespace Yanyana.BackEnd.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatesController : ControllerBase
    {
        private readonly IRateManager _rateManager;

        public RatesController(IRateManager rateManager)
        {
            _rateManager = rateManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rate>>> GetAllRates()
        {
            return Ok(await _rateManager.GetAllRatesAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Rate>> GetRateById(int id)
        {
            var rate = await _rateManager.GetRateByIdAsync(id);
            if (rate == null) return NotFound();
            return Ok(rate);
        }

        [HttpGet("place/{placeId}")]
        public async Task<ActionResult<IEnumerable<Rate>>> GetRatesByPlaceId(int placeId)
        {
            return Ok(await _rateManager.GetRatesByPlaceIdAsync(placeId));
        }

        [HttpPost]
        public async Task<ActionResult<Rate>> CreateRate(Rate rate)
        {
            await _rateManager.CreateRateAsync(rate);
            return CreatedAtAction(nameof(GetRateById), new { id = rate.RateId }, rate);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRate(int id, Rate updatedRate)
        {
            if (id != updatedRate.RateId) return BadRequest();
            await _rateManager.UpdateRateAsync(updatedRate);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRate(int id)
        {
            await _rateManager.DeleteRateAsync(id);
            return NoContent();
        }
    }
}