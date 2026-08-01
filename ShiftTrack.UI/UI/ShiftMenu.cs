using ShiftTrack.Models;
using ShiftTrack.UI.DTO.Employee;
using ShiftTrack.UI.DTO.Shifts;
using ShiftTrack.UI.Services;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ShiftTrack.UI.UI
{
    public class ShiftMenu
    {
        private readonly ShiftApiService _shiftApiService;
        private readonly EmployeeApiService _employeeApiService;
        private readonly ConsoleUI _ui;

        public ShiftMenu()
        {
            _shiftApiService = new ShiftApiService();
            _employeeApiService = new EmployeeApiService();
            _ui = new ConsoleUI();
        }

        public async Task ListShiftsAsync()
        {
            try
            {
                var shifts = await AnsiConsole.Status()
                .StartAsync("Fetching shifts...", async _ => await _shiftApiService.GetAllAsync());

                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("ID");
                table.AddColumn("Employee");
                table.AddColumn("Clock In");
                table.AddColumn("Clock Out");
                table.AddColumn("Status");
                table.AddColumn("Notes");

                foreach (var s in shifts)
                {
                    table.AddRow(
                        s.ShiftId.ToString(),
                        Markup.Escape(s.EmployeeName),
                        s.ClockInTime.ToString("g"),
                        s.ClockOutTime?.ToString("g") ?? "-",
                        StatusMarkup(s.Status),
                        Markup.Escape(s.Notes));
                }

                AnsiConsole.Write(table);
                _ui.Pause();
            }
            catch (Exception ex)
            {
                _ui.ShowError($"Error fetching shifts: {ex.Message}");
                _ui.Pause();
            }
        }

        public async Task GetShiftByEmployeeAsync()
        {
            try
            {
                var employees = await _employeeApiService.GetAllAsync();

                if (!employees.Any())
                {
                    _ui.ShowInfo("No employees found.");
                    _ui.Pause();
                    return;
                }

                var selectedEmployee = AnsiConsole.Prompt(
                    new SelectionPrompt<EmployeeDto>()
                        .Title("Select an employee")
                        .UseConverter(e => e.Name)
                        .AddChoices(employees));

                var shifts = await _shiftApiService.GetAllAsync();

                var employeeShifts = shifts
                    .Where(s => s.EmployeeId == selectedEmployee.EmployeeId)
                    .ToList();

                if (!employeeShifts.Any())
                {
                    _ui.ShowInfo($"No shifts found for {selectedEmployee.Name}.");
                    _ui.Pause();
                    return;
                }

                var table = new Table().Border(TableBorder.Rounded);
                table.AddColumn("Shift ID");
                table.AddColumn("Clock In");
                table.AddColumn("Clock Out");
                table.AddColumn("Status");
                table.AddColumn("Notes");

                foreach (var shift in employeeShifts)
                {
                    table.AddRow(
                        shift.ShiftId.ToString(),
                        shift.ClockInTime.ToString("g"),
                        shift.ClockOutTime?.ToString("g") ?? "-",
                        StatusMarkup(shift.Status),
                        Markup.Escape(shift.Notes));
                }

                AnsiConsole.Write(table);
                _ui.Pause();
            }
            catch (Exception ex)
            {
                _ui.ShowError($"Error fetching shifts: {ex.Message}");
                _ui.Pause();
            }
        }

        public async Task CreateShiftAsync()
        {
            try
            {
                var employees = await _employeeApiService.GetAllAsync();

                if (!employees.Any())
                {
                    _ui.ShowInfo("No employees found.");
                    _ui.Pause();
                    return;
                }

                var selectedEmployee = AnsiConsole.Prompt(
                    new SelectionPrompt<EmployeeDto>()
                        .Title("Select an employee")
                        .UseConverter(e => e.Name)
                        .AddChoices(employees));
                var notes = AnsiConsole.Ask("Notes [grey](optional)[/]:", string.Empty);

                var success = await AnsiConsole.Status()
                    .StartAsync("Creating shift...", async _ => await _shiftApiService.CreateAsync(selectedEmployee.EmployeeId, notes));

                if (success)
                    _ui.ShowSuccess("Shift created.");
                else
                    _ui.ShowError("Failed to create shift.");
                _ui.Pause();
            }
            catch(Exception ex)
            {
                _ui.ShowError($"Error creating shift: {ex.Message}");
                _ui.Pause();
            }
        }

        public async Task UpdateShiftAsync()
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
                        .Title("Select an [green]employee[/]:")
                        .PageSize(10)
                        .UseConverter(e => e.Name)
                        .AddChoices(employees));

                var shifts = (await _shiftApiService.GetAllAsync())
                    .Where(s => s.EmployeeId == selectedEmployee.EmployeeId)
                    .ToArray();

                if (shifts.Length == 0)
                {
                    _ui.ShowInfo($"{selectedEmployee.Name} has no shifts.");
                    _ui.Pause();
                    return;
                }

                var selectedShift = AnsiConsole.Prompt(
                    new SelectionPrompt<ShiftDto>()
                        .Title("Select a [green]shift[/] to update:")
                        .PageSize(10)
                        .UseConverter(s =>
                            $"ID: {s.ShiftId} | {s.ClockInTime:g} | {s.Status}")
                        .AddChoices(shifts));

                ShiftStatus? status = null;
                if (AnsiConsole.Confirm("Update status?", false))
                {
                    status = AnsiConsole.Prompt(
                        new SelectionPrompt<ShiftStatus>()
                            .Title("New status:")
                            .AddChoices(Enum.GetValues<ShiftStatus>()));
                }

                string? notes = null;
                if (AnsiConsole.Confirm("Update notes?", false))
                {
                    notes = AnsiConsole.Ask<string>("New [green]notes[/]:");
                }

                DateTime? clockOutTime = null;
                if (AnsiConsole.Confirm("Set clock-out time?", false))
                {
                    clockOutTime = AnsiConsole.Confirm("Use current time?", true)
                        ? DateTime.UtcNow
                        : AnsiConsole.Ask<DateTime>("Clock-out time [grey](e.g. 2026-08-01 17:30)[/]:");
                }

                var success = await AnsiConsole.Status()
                    .StartAsync("Updating shift...",
                        async _ => await _shiftApiService.UpdateAsync(
                            selectedShift.ShiftId,
                            status,
                            notes,
                            clockOutTime));

                if (success)
                    _ui.ShowSuccess("Shift updated.");
                else
                    _ui.ShowError("Failed to update shift.");

                _ui.Pause();
            }
            catch (Exception ex)
            {
                _ui.ShowError($"Error updating shift: {ex.Message}");
                _ui.Pause();
            }
        }

        public async Task DeleteShiftAsync()
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
                        .Title("Select an [green]employee[/]:")
                        .PageSize(10)
                        .UseConverter(e => e.Name)
                        .AddChoices(employees));

                var shifts = (await _shiftApiService.GetAllAsync())
                    .Where(s => s.EmployeeId == selectedEmployee.EmployeeId)
                    .ToArray();

                if (shifts.Length == 0)
                {
                    _ui.ShowInfo($"{selectedEmployee.Name} has no shifts.");
                    _ui.Pause();
                    return;
                }

                var selectedShift = AnsiConsole.Prompt(
                    new SelectionPrompt<ShiftDto>()
                        .Title("Select a [green]shift[/] to delete:")
                        .PageSize(10)
                        .UseConverter(s =>
                            $"ID: {s.ShiftId} | {s.ClockInTime:g} | {s.Status}")
                        .AddChoices(shifts));
                if (!AnsiConsole.Confirm($"Are you sure you want to delete shift {selectedShift.ShiftId}?", false))
                {
                    _ui.ShowInfo("Deletion cancelled.");
                    _ui.Pause();
                    return;
                }
                var success = await AnsiConsole.Status()
                    .StartAsync("Deleting shift...", async _ => await _shiftApiService.DeleteAsync(selectedShift.ShiftId));
                if (success)
                    _ui.ShowSuccess("Shift deleted.");
                else
                    _ui.ShowError("Failed to delete shift.");
                _ui.Pause();
            }
            catch(Exception ex)
            {
                _ui.ShowError($"Error deleting shift: {ex.Message}");
                _ui.Pause();
            }
        }

        public  string StatusMarkup(ShiftStatus status)
        {
            var color = status switch
            {
                ShiftStatus.Scheduled => "grey",
                ShiftStatus.InProgress => "yellow",
                ShiftStatus.Completed => "green",
                ShiftStatus.Cancelled => "red",
                ShiftStatus.Missed => "red",
                _ => "white"
            };

            return $"[{color}]{status}[/]";
        }
    }
}

