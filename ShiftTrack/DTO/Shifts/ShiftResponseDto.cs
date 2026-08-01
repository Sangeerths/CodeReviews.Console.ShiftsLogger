using ShiftTrack.Models;

namespace ShiftTrack.DTO.Shifts
{
    public class ShiftResponseDto
    {
        public int ShiftId { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty; 
        public DateTime ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public string Notes { get; set; } = string.Empty;
        public ShiftStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
