using Microsoft.AspNetCore.Mvc;
using ShiftTrack.DTO.Shifts;
using ShiftTrack.Models;
using ShiftTrack.Services;

namespace ShiftTrack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _shiftService.GetAllShiftsAsync());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var shift = await _shiftService.GetShiftByIdAsync(id);

            if (shift == null)
                return NotFound();

            return Ok(shift);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateShiftsDto shift)
        {
            var createdShift = await _shiftService.CreateShiftAsync(shift);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createdShift.ShiftId },
                createdShift);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateShiftsDto shift)
        {
            var updated = await _shiftService.UpdateShiftAsync(shift);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _shiftService.DeleteShiftAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
