using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShiftTrack.UI.DTO.Employee
{
    public class UpdateEmployeeDto
    {
        [Range(1, int.MaxValue, ErrorMessage = "EmployeeId must be a positive number.")]
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Employee name must be between 1 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
