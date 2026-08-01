using ShiftTrack.UI.DTO.Employee;
using System.Net.Http.Json;

namespace ShiftTrack.UI.Services;

public class EmployeeApiService
{
    private readonly HttpClient _client;
    private readonly ValidationService _validationService;

    public EmployeeApiService()
    {
        _client = new HttpClient
        {
            BaseAddress = new Uri("https://localhost:7098/")
        };
        _validationService = new ValidationService();
    }

    public async Task<EmployeeDto[]> GetAllAsync()
    {
        try
        {
            var employees = await _client.GetFromJsonAsync<EmployeeDto[]>("api/Employee");
            return employees ?? Array.Empty<EmployeeDto>();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<EmployeeDto?> GetByIdAsync(int id)
    {
        try
        {
            _validationService.ValidateId(id);
            var response = await _client.GetAsync($"api/Employee/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<EmployeeDto>();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> CreateAsync(CreateEmployeeDto dto)
    {
        try
        {
            _validationService.ValidateName(dto.Name);

            var payload = new
            {
                Name = dto.Name.Trim()
            };
            var response = await _client.PostAsJsonAsync("api/Employee", payload);
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> UpdateAsync(UpdateEmployeeDto dto)
    {
        try
        {
            _validationService.ValidateId(dto.EmployeeId);
            _validationService.ValidateName(dto.Name);

            var payload = new
            {
                EmployeeId = dto.EmployeeId,
                Name = dto.Name.Trim(),
                UpdatedAt = DateTime.UtcNow
            };
            var response = await _client.PutAsJsonAsync($"api/Employee", payload);
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
            var response = await _client.DeleteAsync($"api/Employee/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
