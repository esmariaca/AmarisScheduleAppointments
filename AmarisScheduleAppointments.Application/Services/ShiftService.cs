using AmarisScheduleAppointments.Application.Interfaces;
using AmarisScheduleAppointments.Domain.Entities;
using AmarisScheduleAppointments.Infrastructure.DataBase;
using Microsoft.EntityFrameworkCore;

namespace AmarisScheduleAppointments.Application.Services
{
    public class ShiftService : IShiftService
    {
        private readonly AppDbContext _context;

        public ShiftService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Shift> RegisterShiftAsync(Shift shift)
        {
            var today = DateTime.Today;

            var count = await _context.Shifts
                .CountAsync(s => s.IdentificationNumber == shift.IdentificationNumber
                            && s.CreatedAt.Date == today);

            if (count >= 5)
            {
                throw new InvalidOperationException("Ya has solicitado el máximo de 5 turnos permitidos por hoy.");
            }

            shift.CreatedAt = DateTime.Now;
            shift.LimitTime = shift.CreatedAt.AddMinutes(15);
            shift.Status = "Pending";

            _context.Shifts.Add(shift);
            await _context.SaveChangesAsync();
            return shift;
        }

        public async Task<IEnumerable<Shift>> GetAllShiftsAsync()
        {
            var now = DateTime.Now;
            var expiredShifts = await _context.Shifts
                .Where(s => s.Status == "Pending" && now > s.LimitTime)
                .ToListAsync();

            if (expiredShifts.Any())
            {
                foreach (var s in expiredShifts) s.Status = "Expired";
                await _context.SaveChangesAsync();
            }

            return await _context.Shifts.Include(s => s.Branch).ToListAsync();
        }

        public async Task<Shift?> GetShiftByIdAsync(int id)
        {
            return await _context.Shifts.Include(s => s.Branch)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task UpdateShiftAsync(Shift shift)
        {
            _context.Entry(shift).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
