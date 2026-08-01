using Microsoft.AspNetCore.Mvc;
using ShiftTrack.DTO.Employees;
using ShiftTrack.Services;
namespace ShiftTrack.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeeController : ControllerBase
{
    private readonly IEmployeeService _employeeService;

    public EmployeeController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _employeeService.GetAllEmployeesAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var shift = await _employeeService.GetEmployeeByIdAsync(id);

        if (shift == null)
            return NotFound();

        return Ok(shift);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateEmployeeDto employee)
    {
        try
        {
            var createdEmployee = await _employeeService.CreateEmployeeAsync(employee);
            return Ok(createdEmployee);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.ToString());
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateEmployeeDto employee)
    {
       
        var updated = await _employeeService.UpdateEmployeeAsync( employee);

        if (!updated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _employeeService.DeleteEmployeeAsync(id);

        if (!deleted)
            return NotFound();

        return NoContent();
    }
}
