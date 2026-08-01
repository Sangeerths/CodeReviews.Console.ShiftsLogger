namespace ShiftTrack.Models;

public class Employee
{
    public int EmployeeId { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public ICollection<Shift> Shifts { get; set; } = new List<Shift>();
}
