using ShiftTrack.UI.DTO.Employee;
using ShiftTrack.UI.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftTrack.UI.UI
{
    public class EmployeeMenu
    {
        private readonly EmployeeApiService _employeeApiService;
        private readonly ConsoleUI _ui;

        public EmployeeMenu()
        {
            _employeeApiService = new EmployeeApiService();
            _ui = new ConsoleUI();
        }

        public async Task ListEmployeesAsync()
        {
            try
            {
                var employees = await AnsiConsole.Status()
               .StartAsync("Fetching employees...", async _ => await _employeeApiService.GetAllAsync());

                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("ID");
                table.AddColumn("Name");

                foreach (var e in employees)
                {
                    table.AddRow(e.EmployeeId.ToString(), Markup.Escape(e.Name));
                }

                AnsiConsole.Write(table);
                _ui.Pause();
            }
            catch (Exception ex)
            {
                _ui.ShowError($"Error fetching employees: {ex.Message}");
                _ui.Pause();
            }
           
        }

        public async Task GetEmployeeByIdAsync()
        {
            try
            {
                var id = AnsiConsole.Ask<int>("Employee [green]ID[/]:");

                var employee = await AnsiConsole.Status()
                    .StartAsync("Looking up employee...", async _ => await _employeeApiService.GetByIdAsync(id));

                if (employee == null)
                {
                    _ui.ShowError("Employee not found.");
                    _ui.Pause();
                    return;
                }

                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("ID");
                table.AddColumn("Name");
                table.AddRow(employee.EmployeeId.ToString(), Markup.Escape(employee.Name));
                AnsiConsole.Write(table);
                _ui.Pause();
            }
            catch (Exception ex)
            {
                _ui.ShowError($"Error fetching employee: {ex.Message}");
                _ui.Pause();
            }

        }

        public async Task CreateEmployeeAsync()
        {
            try
            {
                CreateEmployeeDto employee = new CreateEmployeeDto
                {
                    Name = AnsiConsole.Ask<string>("Employee [green]name[/]:")
                };

                var success = await AnsiConsole.Status()
                    .StartAsync("Creating employee...", async _ => await _employeeApiService.CreateAsync(employee));

                if (success)
                    _ui.ShowSuccess("Employee created.");
                else
                    _ui.ShowError("Failed to create employee.");
                _ui.Pause();
            }
            catch (Exception ex)
            {
                _ui.ShowError($"Error creating employee: {ex.Message}");
                _ui.Pause();
            }

        }

        public async Task UpdateEmployeeAsync()
        {
            try
            {
                var employees = await _employeeApiService.GetAllAsync();

                if (employees.Length == 0)
                {
                    _ui.ShowError("No employees found.");
                    _ui.Pause();
                    return;
                }

                var selectedEmployee = AnsiConsole.Prompt(
                    new SelectionPrompt<EmployeeDto>()
                        .Title("Select an [green]employee[/] to update:")
                        .PageSize(10)
                        .UseConverter(e => $"{e.Name} (ID: {e.EmployeeId})")
                        .AddChoices(employees));

                var employee = new UpdateEmployeeDto
                {
                    EmployeeId = selectedEmployee.EmployeeId,
                    Name = AnsiConsole.Ask<string>(
                        $"New name for [green]{selectedEmployee.Name}[/]:")
                };

                var success = await AnsiConsole.Status()
                    .StartAsync("Updating employee...",
                        async _ => await _employeeApiService.UpdateAsync(employee));

                if (success)
                    _ui.ShowSuccess("Employee updated.");
                else
                    _ui.ShowError("Failed to update employee.");

                _ui.Pause();
            }
            catch (Exception ex)
            {
                _ui.ShowError($"Error updating employee: {ex.Message}");
                _ui.Pause();
            }
        }

        public async Task DeleteEmployeeAsync()
        {
            try
            {
                var employees = await _employeeApiService.GetAllAsync();

                if (employees.Length == 0)
                {
                    _ui.ShowError("No employees found.");
                    _ui.Pause();
                    return;
                }

                var selectedEmployee = AnsiConsole.Prompt(
                    new SelectionPrompt<EmployeeDto>()
                        .Title("Select an [green]employee[/] to update:")
                        .PageSize(10)
                        .UseConverter(e => $"{e.Name} (ID: {e.EmployeeId})")
                        .AddChoices(employees));
                var success = await AnsiConsole.Status()
                    .StartAsync("Deleting employee...", async _ => await _employeeApiService.DeleteAsync(selectedEmployee.EmployeeId));
                if (success)
                    _ui.ShowSuccess("Employee deleted.");
                else
                    _ui.ShowError("Failed to delete employee.");
                _ui.Pause();
            }
            catch (Exception ex)
            {
                _ui.ShowError($"Error deleting employee: {ex.Message}");
                _ui.Pause();
            }
        }

    }
}
