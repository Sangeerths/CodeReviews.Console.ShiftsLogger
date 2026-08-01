using ShiftTrack.DTO.Employees;
using ShiftTrack.Models;

namespace ShiftTrack.Services
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee?> GetEmployeeByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(CreateEmployeeDto employee);
        Task<bool> UpdateEmployeeAsync(UpdateEmployeeDto employee);
        Task<bool> DeleteEmployeeAsync(int id);
    }
}
