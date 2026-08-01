using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ShiftTrack.UI.DTO.Employee
{
    public class CreateEmployeeDto
    {
        [Required(ErrorMessage = "Employee name is required.")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Employee name must be between 1 and 100 characters.")]
        public string Name { get; set; } = string.Empty;
    }
}
