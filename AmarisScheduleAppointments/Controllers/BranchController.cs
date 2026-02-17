using AmarisScheduleAppointments.Infrastructure.DataBase;
using AmarisScheduleAppointments.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AmarisScheduleAppointments.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BranchController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Branch>>> GetBranches()
        {
            var branches = await _context.Branches.ToListAsync();
            return Ok(branches);
        }
    }
}