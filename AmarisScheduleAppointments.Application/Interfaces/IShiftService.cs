using AmarisScheduleAppointments.Domain.Entities;

namespace AmarisScheduleAppointments.Application.Interfaces
{
    public interface IShiftService
    {
        Task<Shift> RegisterShiftAsync(Shift shift);
        Task<IEnumerable<Shift>> GetAllShiftsAsync();
        Task<Shift?> GetShiftByIdAsync(int id);
        Task UpdateShiftAsync(Shift shift);
    }
}
