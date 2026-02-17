using System.Text.Json.Serialization;

namespace AmarisScheduleAppointments.Domain.Entities
{
    public class Branch
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
    }
}
