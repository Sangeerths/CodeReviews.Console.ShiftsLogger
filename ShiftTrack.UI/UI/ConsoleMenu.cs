using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShiftTrack.UI.UI
{
    public class ConsoleMenu
    {
        private readonly EmployeeMenu _employeeMenu;
        private readonly ShiftMenu _shiftMenu;
        private readonly ConsoleUI _ui;
        private enum MenuOptions
        {
            EmployeeManagement,
            ShiftManagement,
            Exit
        }

        public ConsoleMenu()
        {
            _employeeMenu = new EmployeeMenu();
            _shiftMenu = new ShiftMenu();
            _ui = new ConsoleUI();
        }

        public async Task OnStart()
        {
            bool isRunning = true;
            while (isRunning)
            {
                _ui.ShowHeader();
                var choice = AnsiConsole.Prompt(new SelectionPrompt<MenuOptions>().Title("Choose your operation").AddChoices(
                MenuOptions.EmployeeManagement,
                MenuOptions.ShiftManagement,
                MenuOptions.Exit));

                switch (choice)
                {
                    case MenuOptions.EmployeeManagement:
                        await EmployeeManagementAsync();
                        break;
                    case MenuOptions.ShiftManagement:
                        await ShiftManagementAsync();
                        break;
                    case MenuOptions.Exit:
                        isRunning = false;
                        break;
                }
            }
            _ui.ShowGoodbye();
            _ui.Pause();
        }

        public async Task EmployeeManagementAsync()
        {
            bool isSubMenuRunning = true;
            while (isSubMenuRunning)
            {
                _ui.ShowHeader();
                var choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Employee Management Menu").AddChoices(
                    "List Employees",
                    "Get Employee by ID",
                    "Create Employee",
                    "Update Employee",
                    "Delete Employee",
                    "Back to Main Menu"));
                switch (choice)
                {
                    case "List Employees":
                        await _employeeMenu.ListEmployeesAsync();
                        break;
                    case "Get Employee by ID":
                        await _employeeMenu.GetEmployeeByIdAsync();
                        break;
                    case "Create Employee":
                        await _employeeMenu.CreateEmployeeAsync();
                        break;
                    case "Update Employee": 
                        await _employeeMenu.UpdateEmployeeAsync();
                        break;
                    case "Delete Employee":
                        await _employeeMenu.DeleteEmployeeAsync();
                        break;
                    case "Back to Main Menu":
                        isSubMenuRunning = false;
                        break;
                }
            }
        }

        public async Task ShiftManagementAsync()
        {
            bool isSubMenuRunning = true;
            while (isSubMenuRunning)
            {
                _ui.ShowHeader();
                var choice = AnsiConsole.Prompt(new SelectionPrompt<string>().Title("Shift Management Menu").AddChoices(
                    "List Shifts",
                    "Get Shift by ID",
                    "Create Shift",
                    "Update Shift",
                    "Delete Shift",
                    "Back to Main Menu"));
                switch (choice)
                {
                    case "List Shifts":
                        await _shiftMenu.ListShiftsAsync();
                        break;
                    case "Get Shift by ID":
                        await _shiftMenu.GetShiftByEmployeeAsync();
                        break;
                    case "Create Shift":
                        await _shiftMenu.CreateShiftAsync();
                        break;
                    case "Update Shift":
                        await _shiftMenu.UpdateShiftAsync();
                        break;
                    case "Delete Shift":
                        await _shiftMenu.DeleteShiftAsync();
                        break;
                    case "Back to Main Menu":
                        isSubMenuRunning = false;
                        break;
                }
            }
        }
    }
}
