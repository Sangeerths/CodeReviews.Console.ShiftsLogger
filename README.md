# ShiftTrack

A simple, no-fuss app for logging and tracking work shifts. Clock in, clock out, and instantly see your hours, schedules, and earnings in one place — no spreadsheets, no guesswork.

The solution has two projects:

- **`ShiftTrack`** — the ASP.NET Core Web API (EF Core, SQL Server) that owns the `Employees` and `Shifts` data.
- **`ShiftTrack.UI`** — a console client, built with [Spectre.Console](https://spectreconsole.net/), for managing employees and shifts against the API.

## Features

- **Employees** — list, get by ID, create, update, delete
- **Shifts** — list, get by ID, create, update, delete
- Client-side validation (`ValidationService`) before any request hits the API
- Styled console UI: tables, status spinners, headers, and colored success/error/info/warning messages

## Requirements

- .NET SDK matching the project's target framework
- SQL Server (or whatever provider the `ShiftTrack` API's `DbContext` is configured for)

## Getting Started

### 1. Run the API

```
cd ShiftTrack
dotnet run
```

By default the console client expects the API at `https://localhost:7098/`. If your API runs on a different port, update the `BaseAddress` in `ShiftTrack.UI/Services/EmployeeApiService.cs` and `ShiftApiService.cs`.

### 2. Run the console client

```
cd ShiftTrack.UI
dotnet run
```

## Project Structure

```
ShiftTrack/                          # Web API
  Controllers/
  Services/
  DTO/
  Models/

ShiftTrack.UI/                       # Console client
  UI/
    ConsoleMenu.cs                   # Top-level menu (Employee / Shift Management / Exit)
    EmployeeMenu.cs                  # Employee submenu + actions
    ShiftMenu.cs                     # Shift submenu + actions
    ConsoleUI.cs                     # Header, messages, pause, confirm, goodbye
  Services/
    EmployeeApiService.cs            # HTTP calls for employees
    ShiftApiService.cs               # HTTP calls for shifts
    ValidationService.cs             # Shared input validation
  DTO/
    Employee/
      EmployeeDto.cs
      CreateEmployeeDto.cs
      UpdateEmployeeDto.cs
    Shifts/
      ShiftDto.cs
      ShiftStatus.cs
  Program.cs                         # Entry point (top-level statements)
```

## Validation

`ValidationService` is shared by both API services and runs before any HTTP call:

- IDs must be positive numbers
- Employee names are required, max 100 characters, letters and spaces only
- Shift notes are capped at 500 characters

Validation failures throw `ArgumentException`, which each menu action catches and displays via `ConsoleUI.ShowError`.


