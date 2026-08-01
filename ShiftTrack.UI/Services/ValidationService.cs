using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftTrack.UI.Services
{
    public class ValidationService
    {
        private const int MaxNameLength = 100;
        private const int MaxNotesLength = 500;
        public ValidationService() { }
        public void ValidateId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Employee ID must be a positive number.");
        }

        public void ValidateName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Employee name is required.");

            name = name.Trim();

            if (name.Length > MaxNameLength)
                throw new ArgumentException($"Employee name cannot exceed {MaxNameLength} characters.");

            if (name.Any(char.IsDigit))
                throw new ArgumentException("Employee name cannot contain numbers.");

            if (!name.All(c => char.IsLetter(c) || c == ' '))
                throw new ArgumentException("Employee name can only contain letters and spaces.");
        }

        public void ValidateNotes(string? notes)
        {
            if (notes != null && notes.Length > MaxNotesLength)
                throw new ArgumentException($"Notes cannot exceed {MaxNotesLength} characters.");
        }
    }
}
