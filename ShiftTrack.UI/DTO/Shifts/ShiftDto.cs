using ShiftTrack.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftTrack.UI.DTO.Shifts
{
    public class ShiftDto
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
