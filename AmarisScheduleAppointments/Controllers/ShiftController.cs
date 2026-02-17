using AmarisScheduleAppointments.Application.Interfaces;
using AmarisScheduleAppointments.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace AmarisScheduleAppointments.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpPost]
        public async Task<ActionResult<Shift>> CreateShift(Shift newShift)
        {
            try
            {
                var createdShift = await _shiftService.RegisterShiftAsync(newShift);
                return Ok(createdShift);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shift>>> GetShifts()
        {
            var shifts = await _shiftService.GetAllShiftsAsync();
            return Ok(shifts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Shift>> GetShiftById(int id)
        {
            var shift = await _shiftService.GetShiftByIdAsync(id);
            if (shift == null) return NotFound("Turno no encontrado.");

            return Ok(shift);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateShift(int id, Shift updatedShift)
        {
            if (id != updatedShift.Id) return BadRequest("El ID no coincide.");

            await _shiftService.UpdateShiftAsync(updatedShift);
            return NoContent();
        }
    }
}