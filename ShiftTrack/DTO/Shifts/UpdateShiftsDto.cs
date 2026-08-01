using ShiftTrack.Models;

namespace ShiftTrack.DTO.Shifts
{
    public class UpdateShiftsDto
    {
        public int ShiftId { get; set; }
        public int? EmployeeId { get; set; }
        public DateTime? ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public string? Notes { get; set; }
        public ShiftStatus? Status { get; set; }
    }
}
