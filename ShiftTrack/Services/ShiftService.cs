using Microsoft.EntityFrameworkCore;
using ShiftTrack.Data;
using ShiftTrack.DTO.Shifts;
using ShiftTrack.Models;

namespace ShiftTrack.Services;

public class ShiftService: IShiftService
{
    private readonly ShiftDbContext _dbContext;

    public ShiftService(ShiftDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<ShiftResponseDto>> GetAllShiftsAsync()
    {
        return await _dbContext.Shifts
        .Select(s => new ShiftResponseDto
        {
            ShiftId = s.ShiftId,
            EmployeeId = s.EmployeeId,
            EmployeeName = s.Employee.Name,
            ClockInTime = s.ClockInTime,
            ClockOutTime = s.ClockOutTime,
            Notes = s.Notes,
            Status = s.Status,
            CreatedAt = s.CreatedAt
        })
        .ToListAsync();
    }

    public async Task<ShiftResponseDto> GetShiftByIdAsync(int id)
    {
        return await _dbContext.Shifts
        .Where(s => s.ShiftId == id)
        .Select(s => new ShiftResponseDto
        {
            ShiftId = s.ShiftId,
            EmployeeId = s.EmployeeId,
            EmployeeName = s.Employee.Name,
            ClockInTime = s.ClockInTime,
            ClockOutTime = s.ClockOutTime,
            Notes = s.Notes,
            Status = s.Status,
            CreatedAt = s.CreatedAt
        })
        .FirstOrDefaultAsync();
    }

    public async Task <Shift> CreateShiftAsync(CreateShiftsDto dto)
    {
        var shift = new Shift
        {
            EmployeeId = dto.EmployeeId,
            ClockInTime = dto.ClockInTime,
            ClockOutTime = dto.ClockOutTime,
            Notes = dto.Notes,
            Status = dto.Status,
            CreatedAt = dto.CreatedAt,
        };
          _dbContext.Shifts.Add(shift);
          await _dbContext.SaveChangesAsync();
          return shift;
    }

    public async Task<bool> UpdateShiftAsync(UpdateShiftsDto shift)
    {
         var existingShift = await _dbContext.Shifts
            .FirstOrDefaultAsync(s => s.ShiftId == shift.ShiftId);

        if (existingShift == null)
        {
            return false;
        }

        if (shift.EmployeeId.HasValue)
            existingShift.EmployeeId = shift.EmployeeId.Value;

        if (shift.ClockInTime.HasValue)
            existingShift.ClockInTime = shift.ClockInTime.Value;

        if (shift.ClockOutTime.HasValue)
            existingShift.ClockOutTime = shift.ClockOutTime;

        if (shift.Notes != null)
            existingShift.Notes = shift.Notes;

        if (shift.Status.HasValue)
            existingShift.Status = shift.Status.Value;

        existingShift.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteShiftAsync(int id)
    {
        var shift = await _dbContext.Shifts
            .FirstOrDefaultAsync(s => s.ShiftId == id);

        if (shift == null)
        {
            return false;
        }

        _dbContext.Shifts.Remove(shift);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}
