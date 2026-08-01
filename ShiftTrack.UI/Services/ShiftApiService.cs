using ShiftTrack.Models;
using ShiftTrack.UI.DTO.Shifts;
using System.Net.Http.Json;
namespace ShiftTrack.UI.Services;

public class ShiftApiService
{
    private readonly HttpClient _client;
    private readonly ValidationService _validationService;

    public ShiftApiService()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7098/")
        };
        _validationService = new ValidationService();
    }


    public async Task<ShiftDto[]> GetAllAsync()
    {
        try
        {
            var shifts = await _client.GetFromJsonAsync<ShiftDto[]>("api/Shift");
            return shifts ?? Array.Empty<ShiftDto>();
        }
        catch(Exception)
        {
            throw;
        }
    }

    public async Task<ShiftDto?> GetByIdAsync(int id)
    {
        try
        {
            _validationService.ValidateId(id);

            var response = await _client.GetAsync($"api/Shift/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<ShiftDto>();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> CreateAsync(int employeeId, string notes)
    {
        try
        {
            _validationService.ValidateId(employeeId);
            _validationService.ValidateNotes(notes);

            var payload = new
            {
                EmployeeId = employeeId,
                ClockInTime = DateTime.UtcNow,
                ClockOutTime = (DateTime?)null,
                Notes = notes.Trim(),
                Status = ShiftStatus.InProgress,
                CreatedAt = DateTime.UtcNow
            };

            var response = await _client.PostAsJsonAsync("api/Shift", payload);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> UpdateAsync(int id, ShiftStatus? status, string? notes, DateTime? clockOutTime = null)
    {
        try
        {
            _validationService.ValidateId(id);

            if (status == null && notes == null && clockOutTime == null)
                throw new ArgumentException("Nothing to update — provide a status, notes, or clock-out time.");

            if (status.HasValue && !Enum.IsDefined(status.Value))
                throw new ArgumentException("Invalid shift status.");

            if (notes != null)
                _validationService.ValidateNotes(notes);

            var payload = new
            {
                ShiftId = id,
                Status = status,
                Notes = notes?.Trim(),
                ClockOutTime = clockOutTime
            };

            var response = await _client.PutAsJsonAsync($"api/Shift", payload);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            _validationService.ValidateId(id);
            var response = await _client.DeleteAsync($"api/Shift/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
