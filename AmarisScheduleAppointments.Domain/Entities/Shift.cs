using System.ComponentModel.DataAnnotations;

namespace AmarisScheduleAppointments.Domain.Entities
{
    public class Shift
    {
        public int Id { get; set; }

        [Required]
        public string IdentificationNumber { get; set; } = string.Empty;
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime LimitTime { get; set; }
        public string Status { get; set; } = "Pending";
    }
}
