namespace ShiftTrack.Models;

public class Shift
{
    public int ShiftId { get; set; }
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
    public DateTime ClockInTime { get; set; }
    public DateTime? ClockOutTime { get; set; }
    public string Notes { get; set; } = string.Empty;
    public ShiftStatus Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
