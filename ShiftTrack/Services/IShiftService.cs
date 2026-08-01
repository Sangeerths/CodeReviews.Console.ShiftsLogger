using ShiftTrack.DTO.Shifts;
using ShiftTrack.Models;

namespace ShiftTrack.Services
{
    public interface IShiftService
    {
        Task<List<ShiftResponseDto>> GetAllShiftsAsync();
        Task<ShiftResponseDto?> GetShiftByIdAsync(int id);
        Task<Shift> CreateShiftAsync(CreateShiftsDto shift);
        Task<bool> UpdateShiftAsync(UpdateShiftsDto shift);
        Task<bool> DeleteShiftAsync(int id);
    }
}
